using Microsoft.EntityFrameworkCore;
using SignalGo.Shared.DataTypes;
using System.Linq;
using Translators.Attributes;
using Translators.Contracts.Common;
using Translators.Contracts.Requests;
using Translators.Database.Contexts;
using Translators.Database.Entities;
using Translators.Helpers;
using Translators.Logics;
using Translators.Validations;

namespace Translators.Services
{
    [ServiceContract("Page", ServiceType.HttpService, InstanceType.SingleInstance)]
    [ServiceContract("Page", ServiceType.ServerService, InstanceType.SingleInstance)]
    public class PageService
    {
        [JsonCustomSerialization]
        public async Task<MessageContract<List<PageContract>>> GetPage([NumberValidation] long pageNumber, [NumberValidation] long bookId)
        {
            var result = await new LogicBase<TranslatorContext, PageContract, PageEntity>().GetAll(x =>
                         x.Include(q => q.Paragraphs).ThenInclude(p => p.Words).ThenInclude(w => w.Values).ThenInclude(n => n.Language)
                         .Include(q => q.Paragraphs).ThenInclude(p => p.Words).ThenInclude(w => w.Values).ThenInclude(n => n.Translator)
                         .Include(q => q.Paragraphs).ThenInclude(p => p.FromLinkParagraphs)
                         .Include(q => q.Paragraphs).ThenInclude(p => p.ToLinkParagraphs)
                        .Where(q => q.Number == pageNumber && q.Catalog.BookId == bookId));
            var catalogIds = result.Result.SelectMany(x => x.Paragraphs).GroupBy(x => x.CatalogId).Select(x => x.Key).ToArray();
            var catalogs = await new LogicBase<TranslatorContext, CatalogContract, CatalogEntity>().GetAll(x => x.Include(c => c.Names).ThenInclude(c => c.Language).Where(i => catalogIds.Contains(i.Id)));
            foreach (var page in result.Result)
            {
                page.CatalogNames = page.Paragraphs.SelectMany(p => catalogs.Result.FirstOrDefault(x => x.Id == p.CatalogId)?.Names).Distinct().ToList();
            }
            return result;
        }

        [JsonCustomSerialization]
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

        [JsonCustomSerialization]
        public async Task<MessageContract<List<SearchValueContract>>> Search(AdvancedSearchFilterRequestContract advancedSearchFilterRequest)
        {
            advancedSearchFilterRequest.Search = TextHelper.FixArabicForSearch(advancedSearchFilterRequest.Search).Trim();
            if (string.IsNullOrEmpty(advancedSearchFilterRequest.Search))
                return ("لطفا متن جستجو را پر کنید!", "");
            string[] searchKeys;
            if (advancedSearchFilterRequest.DoFullWordsSearch)
                searchKeys = new string[] { advancedSearchFilterRequest.Search.Replace("  ", " ").Trim() };
            else
                searchKeys = advancedSearchFilterRequest.Search.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
            if (searchKeys.Length == 0)
                return ("لطفا متن جستجو را پر کنید!", "");
            var bookIds = advancedSearchFilterRequest.BookIds.ToArray();
            var query = CacheLogic.Paragraphs.Values.AsQueryable();
            //if (advancedSearchFilterRequest.SkipSearchInTranslates)
            //{
            //    query = query.Where(x => x.TranslatorId == null);
            //}

            //if (advancedSearchFilterRequest.SkipSearchInMain)
            //{
            //    query = query.Where(x => x.TranslatorId > 0);
            //}

            if (bookIds.Length > 0)
            {
                query = query.Where(x => bookIds.Contains(CacheLogic.Catalogs[CacheLogic.Paragraphs[x.Id].CatalogId].BookId));
            }

            if (advancedSearchFilterRequest.DoFullWordsSearch)
                query = query.Where(x => (!advancedSearchFilterRequest.SkipSearchInMain && x.GetMainSearchValue().Contains(searchKeys[0])) || (!advancedSearchFilterRequest.SkipSearchInTranslates && x.GetTranslatedSearchValue().Contains(searchKeys[0])));
            else
                query = query.Where(x => searchKeys.All(key => x.Words.Any(w => w.Values.Any(v => CanSearch(advancedSearchFilterRequest.SkipSearchInTranslates, advancedSearchFilterRequest.SkipSearchInMain, key, v)))));


            var paragraphs = query.ToList();

            List<SearchValueContract> result = new List<SearchValueContract>();
            foreach (var paragraph in paragraphs)
            {
                //var paragraph = CacheLogic.Paragraphs[CacheLogic.Words[word.WordValueId.Value].ParagraphId];
                var catalog = CacheLogic.Catalogs[paragraph.CatalogId];
                result.Add(new SearchValueContract()
                {
                    HasLink = paragraph.HasLink,
                    Number = paragraph.Number,
                    //Value = word,
                    ParagraphWords = paragraph.Words,
                    ParagraphId = paragraph.Id,
                    PageId = paragraph.PageId,
                    CatalogId = paragraph.CatalogId,
                    CatalogNames = catalog.Names,
                    BookId = catalog.BookId,
                    BookNames = CacheLogic.Books[catalog.BookId].Names,
                });
            }
            return result;
        }
        bool CanSearch(bool skipSearchInTranslates, bool skipSearchInMain, string key, ValueContract value)
        {
            if (!value.IsMain && skipSearchInTranslates)
                return false;
            if (value.IsMain && skipSearchInMain)
                return false;
            return value.SearchValue.Contains(key) || value.Value.Contains(key);
        }
    }
}