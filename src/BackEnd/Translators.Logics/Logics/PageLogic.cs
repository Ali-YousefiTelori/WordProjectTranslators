using Microsoft.EntityFrameworkCore;
using Translators.Contracts.Common;
using Translators.Contracts.Common.Files;
using Translators.Contracts.Common.Paragraphs;
using Translators.Contracts.Responses.Pages;
using Translators.Database.Contexts;
using Translators.Database.Entities;
using Translators.Validations;

namespace Translators.Logics
{
    public class PageLogic
    {
        async Task<MessageContract<(List<PageEntity> Pages, List<CatalogEntity> Catalogs)>> GetPageEntities([NumberValidation] long pageNumber, [NumberValidation] long bookId)
        {
            var result = await new LogicBase<TranslatorContext, PageEntity>().GetAll(x =>
                        x.Include(q => q.Audios)
                        .Include(q => q.Paragraphs).ThenInclude(p => p.Words).ThenInclude(w => w.Values).ThenInclude(n => n.Language)
                        .Include(q => q.Paragraphs).ThenInclude(p => p.Words).ThenInclude(w => w.Values).ThenInclude(n => n.Translator)
                        .Include(q => q.Paragraphs).ThenInclude(p => p.LinkParagraphs)
                        .Include(q => q.Paragraphs).ThenInclude(p => p.Audios)
                       .Where(q => q.Number == pageNumber && q.Catalog.BookId == bookId));
            if (!result)
                return result.ToContract<(List<PageEntity>, List<CatalogEntity>)>();
            var catalogIds = result.Result.SelectMany(x => x.Paragraphs).GroupBy(x => x.CatalogId).Select(x => x.Key).ToArray();
            var catalogs = await new LogicBase<TranslatorContext, CatalogEntity>().GetAll(x => x.Include(c => c.Names).ThenInclude(c => c.Language).Where(i => catalogIds.Contains(i.Id)));
            if (!catalogs)
                return catalogs.ToContract<(List<PageEntity>, List<CatalogEntity>)>();
            return (result.Result, catalogs.Result);
        }

        async Task<MessageContract<(List<PageContract> Pages, List<CatalogContract> Catalogs, string CatalogName)>> GetFastPages([NumberValidation] long pageNumber, [NumberValidation] long bookId)
        {
            var book = CacheLogic.Books[bookId];
            var catalog = CacheLogic.Catalogs.Where(x => x.Value.BookId == bookId).Select(x => x.Value).ToList();
            var pages = CacheLogic.Pages.Select(x => x.Value).Where(x => x.Number == pageNumber && catalog.Any(c => c.Id == x.CatalogId));

            var catalogIds = pages.SelectMany(x => x.Paragraphs).GroupBy(x => x.CatalogId).Select(x => x.Key).ToArray();
            var catalogs = CacheLogic.Catalogs.Select(x => x.Value).Where(i => catalogIds.Contains(i.Id));
            var category = CacheLogic.Categories.Select(x => x.Value).Where(i => i.Books.Any(x => x.Id == bookId)).FirstOrDefault();

            string catalogName = $"{category.Names.GetPersianValue()} / ";
            var value = book.Names.GetPersianValue();
            if (!value.Contains(catalogName.Trim().Trim('/').Trim()))
                catalogName += $"{value} / ";
            catalogName += string.Join(" - ", pages.Select(x => x.CatalogNames.GetPersianValue()).Distinct());
            if (!catalogName.Any(x => char.IsDigit(x)))
            {
                catalogName += " - " + pages.FirstOrDefault()?.Number;
            }
            return (pages.ToList(), catalogs.ToList(), catalogName);
        }

        public async Task<MessageContract<List<PageContract>>> GetPage([NumberValidation] long pageNumber, [NumberValidation] long bookId)
        {
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
                    p.PageNumber = CacheLogic.Pages[p.PageId].Number;
                }
                page.CatalogNames = page.Paragraphs.SelectMany(p => catalogs.FirstOrDefault(x => x.Id == p.CatalogId)?.Names).Distinct().MapToList<ValueContract>();
            }
            CacheLogic.FixPageBookIds(pages);
            return pages;
        }

        public async Task<MessageContract<PageResponseContract>> GetSimplePage([NumberValidation] long pageNumber, [NumberValidation] long bookId)
        {
            var pagesResult = await GetFastPages(pageNumber, bookId);
            if (!pagesResult)
                return pagesResult.ToContract<PageResponseContract>();
            var languages = await new TranslatorLogicBase<LanguageEntity>().GetAll();
            if (!languages)
                return languages.ToContract<PageResponseContract>();
            PageResponseContract result = new PageResponseContract()
            {
                CatalogName = pagesResult.Result.CatalogName,
                Languages = languages.MapResultToList<LanguageContract>(),
                Paragraphs = pagesResult.Result.Pages.First().Paragraphs.MapToList<SimpleParagraphContract>(),
                AudioFiles = pagesResult.Result.Pages.First().AudioFiles.MapToList<SimpleFileContract>()
            };
            return result;
        }
    }
}
