using EasyMicroservices.ServiceContracts;
using EasyMicroservices.TranslatorsMicroservice;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Contracts.Common.Files;
using Translators.Contracts.Common.Paragraphs;
using Translators.Contracts.Responses.Pages;
using Translators.Database.Entities;

namespace Translators.Logics
{
    public class PageLogic
    {
        AppUnitOfWork _appUnitOfWork;
        public PageLogic(AppUnitOfWork appUnitOfWork)
        {
            _appUnitOfWork = appUnitOfWork;
        }

        async Task<MessageContract<(List<PageEntity> Pages, List<CatalogEntity> Catalogs)>> GetPageEntities(long pageNumber, long bookId)
        {
            using var context = _appUnitOfWork.GetContext();
            var result = await new LogicBase<PageEntity>(context).GetAll(x =>
                        x.Include(q => q.Audios)
                        .Include(q => q.Paragraphs).ThenInclude(p => p.Words).ThenInclude(w => w.Values).ThenInclude(n => n.Language)
                        .Include(q => q.Paragraphs).ThenInclude(p => p.Words).ThenInclude(w => w.Values).ThenInclude(n => n.Translator)
                        .Include(q => q.Paragraphs).ThenInclude(p => p.LinkParagraphs)
                        .Include(q => q.Paragraphs).ThenInclude(p => p.Audios)
                       .Where(q => q.Number == pageNumber && q.Catalog.BookId == bookId));
            if (!result)
                return result.ToContract<(List<PageEntity>, List<CatalogEntity>)>();
            var catalogIds = result.Result.SelectMany(x => x.Paragraphs).GroupBy(x => x.CatalogId).Select(x => x.Key).ToArray();
            using var context2 = _appUnitOfWork.GetContext();
            var catalogs = await new LogicBase<CatalogEntity>(context2).GetAll(x => x.Include(c => c.Names).ThenInclude(c => c.Language).Where(i => catalogIds.Contains(i.Id)));
            if (!catalogs)
                return catalogs.ToContract<(List<PageEntity>, List<CatalogEntity>)>();
            return (result.Result, catalogs.Result);
        }

        async Task<MessageContract<(List<PageContract> Pages, List<CatalogContract> Catalogs, string CatalogName)>> GetFastPages(long pageNumber, long bookId)
        {
            var cacheLogic = _appUnitOfWork.GetCacheLogic();
            var book = await cacheLogic.GetBook(bookId);
            var catalog = await cacheLogic.GetCatalogsByBookId(bookId);
            var pages = await cacheLogic.GetPages(pageNumber, catalog.Select(c => c.Id).ToArray());

            var catalogIds = pages.SelectMany(x => x.Paragraphs).GroupBy(x => x.CatalogId)
                .Select(x => x.Key).ToArray();
            var catalogs = await cacheLogic.GetCatalogsByBookIds(catalogIds);
            var category = await cacheLogic.GetCategoryByBookId(bookId);

            string catalogName = $"{category.Names.GetPersianValue()} / ";
            var value = book.Names.GetPersianValue();
            if (!value.Contains(catalogName.Trim().Trim('/').Trim()))
                catalogName += $"{value} / ";
            catalogName += string.Join(" - ", pages.Select(x => x.CatalogNames.GetPersianValue()).Distinct());
            if (!catalogName.Any(x => char.IsDigit(x)))
            {
                catalogName += " - " + pages.FirstOrDefault()?.Number;
            }
            await cacheLogic.FixPageBookIds(pages);
            return (pages.ToList(), catalogs.ToList(), catalogName);
        }

        public async Task<MessageContract<List<PageContract>>> GetPage(long pageNumber, long bookId)
        {
            var cacheLogic = _appUnitOfWork.GetCacheLogic();
            var pagesResult = await GetPageEntities(pageNumber, bookId);
            if (!pagesResult)
                return pagesResult.ToContract<List<PageContract>>();
            var catalogs = pagesResult.Result.Catalogs;
            var pages = pagesResult.Result.Pages.MapToList<PageContract>();
            foreach (var page in pages)
            {
                foreach (var p in page.Paragraphs)
                {
                    var catalog = catalogs.FirstOrDefault(x => x.Id == p.CatalogId);
                    p.BookId = catalog.BookId;
                    p.PageNumber = (await cacheLogic.GetPage(p.PageId)).Number;
                }
                page.CatalogNames = page.Paragraphs.SelectMany(p => catalogs.FirstOrDefault(x => x.Id == p.CatalogId)?.Names).Distinct().MapToList<ValueContract>();
            }
            await cacheLogic.FixPageBookIds(pages);
            return pages;
        }

        public async Task<MessageContract<PageResponseContract>> GetSimplePage(long pageNumber, long bookId)
        {
            using var context = _appUnitOfWork.GetContext();
            var pagesResult = await GetFastPages(pageNumber, bookId);
            if (!pagesResult)
                return pagesResult.ToContract<PageResponseContract>();
            var languages = await new LogicBase<LanguageEntity>(context).GetAll();
            if (!languages)
                return languages.ToContract<PageResponseContract>();
            PageResponseContract result = new PageResponseContract()
            {
                CatalogName = pagesResult.Result.CatalogName,
                Languages = languages.MapResultToList<LanguageContract>(),
                Paragraphs = pagesResult.Result.Pages.First().Paragraphs.MapToList<ParagraphContract>(),
                AudioFiles = pagesResult.Result.Pages.First().AudioFiles.MapToList<AudioFileContract>()
            };
            return result;
        }
    }
}
