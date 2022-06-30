using Microsoft.EntityFrameworkCore;
using SignalGo.Shared.DataTypes;
using Translators.Contracts.Common;
using Translators.Contracts.Requests;
using Translators.Database.Contexts;
using Translators.Database.Entities;
using Translators.Logics;
using Translators.Validations;

namespace Translators.Services
{
    [ServiceContract("Page", ServiceType.HttpService, InstanceType.SingleInstance)]
    [ServiceContract("Page", ServiceType.ServerService, InstanceType.SingleInstance)]
    public class PageService
    {
        public async Task<MessageContract<List<PageContract>>> GetPage([NumberValidation] long pageNumber, [NumberValidation] long bookId)
        {
            var result = await new LogicBase<TranslatorContext, PageContract, PageEntity>().GetAll(x =>
                         x.Include(q => q.Paragraphs).ThenInclude(p => p.Words).ThenInclude(w => w.Values).ThenInclude(n => n.Language)
                         .Include(q => q.Paragraphs).ThenInclude(p => p.Words).ThenInclude(w => w.Values).ThenInclude(n => n.Translator)
                        .Where(q => q.Number == pageNumber && q.Catalog.BookId == bookId));
            var catalogIds = result.Result.SelectMany(x => x.Paragraphs).GroupBy(x => x.CatalogId).Select(x => x.Key).ToArray();
            var catalogs = await new LogicBase<TranslatorContext, CatalogContract, CatalogEntity>().GetAll(x => x.Include(c => c.Names).ThenInclude(c => c.Language).Where(i => catalogIds.Contains(i.Id)));
            foreach (var page in result.Result)
            {
                page.CatalogNames = page.Paragraphs.SelectMany(p => catalogs.Result.FirstOrDefault(x => x.Id == p.CatalogId)?.Names).Distinct().ToList();
            }
            return result;
        }

        public async Task<MessageContract<long>> GetPageNumberByVerseNumber([NumberValidation] long verseNumber, [NumberValidation] long catalogid)
        {
            var verseResult = await new LogicBase<TranslatorContext, ParagraphContract, ParagraphEntity>().Find(x =>
                        x.Where(q => q.Number == verseNumber && q.Page.CatalogId == catalogid));
            if (!verseResult.IsSuccess)
                return verseResult.ToContract<long>();
            var pageResult = await new LogicBase<TranslatorContext, PageContract, PageEntity>().Find(x =>
                       x.Where(q => q.Id == verseResult.Result.PageId));
            if (!pageResult.IsSuccess)
                return pageResult.ToContract<long>();
            return pageResult.Result.Number;
        }

        //public async Task<MessageContract<List<PageContract>>> Search(AdvancedSearchFilterRequestContract advancedSearchFilterRequest)
        //{
        //    var result = await new LogicBase<TranslatorContext, PageContract, PageEntity>().GetAll(x =>
        //                 x.Include(q => q.Paragraphs).ThenInclude(p => p.Words).ThenInclude(w => w.Values).ThenInclude(n => n.Language)
        //                 .Include(q => q.Paragraphs).ThenInclude(p => p.Words).ThenInclude(w => w.Values).ThenInclude(n => n.Translator)
        //                .Where(q => q.Number == pageNumber && q.Catalog.BookId == bookId));
        //    var catalogIds = result.Result.SelectMany(x => x.Paragraphs).GroupBy(x => x.CatalogId).Select(x => x.Key).ToArray();
        //    var catalogs = await new LogicBase<TranslatorContext, CatalogContract, CatalogEntity>().GetAll(x => x.Include(c => c.Names).ThenInclude(c => c.Language).Where(i => catalogIds.Contains(i.Id)));
        //    foreach (var page in result.Result)
        //    {
        //        page.CatalogNames = page.Paragraphs.SelectMany(p => catalogs.Result.FirstOrDefault(x => x.Id == p.CatalogId)?.Names).Distinct().ToList();
        //    }
        //    return result;
        //}
    }
}