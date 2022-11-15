using Microsoft.EntityFrameworkCore;
using SignalGo.Server.Models;
using SignalGo.Shared.DataTypes;
using SignalGo.Shared.Http;
using SignalGo.Shared.Models;
using Translators.Attributes;
using Translators.Contracts.Common;
using Translators.Contracts.Requests;
using Translators.Contracts.Requests.Pages;
using Translators.Contracts.Responses.Pages;
using Translators.Database.Contexts;
using Translators.Database.Entities;
using Translators.Helpers;
using Translators.Logics;
using Translators.Validations;

namespace Translators.Services
{
    [ServiceContract("Page", ServiceType.HttpService, InstanceType.SingleInstance)]
    [ServiceContract("Page", ServiceType.ServerService, InstanceType.SingleInstance)]
    public class ObsoletePageService
    {
        #region Obsolote services
        [JsonCustomSerialization]
        [Obsolete]// performance issue deprecated
        public async Task<MessageContract<List<PageContract>>> GetPage([NumberValidation] long pageNumber, [NumberValidation] long bookId)
        {
            return await new PageLogic().GetPage(pageNumber, bookId);
        }
        #endregion


        [JsonCustomSerialization]
        public async Task<MessageContract<long>> GetPageNumberByVerseNumber([NumberValidation] long verseNumber, [NumberValidation] long catalogId)
        {
            var verseResult = await new LogicBase<TranslatorContext, ParagraphContract, ParagraphEntity>().Find(x =>
                        x.Where(q => q.Number == verseNumber && q.Page.CatalogId == catalogId));
            if (!verseResult.IsSuccess)
                return verseResult.ToContract<long>();
            var pageResult = await new LogicBase<TranslatorContext, PageContract, PageEntity>().Find(x =>
                       x.Where(q => q.Id == verseResult.Result.PageId));
            if (!pageResult.IsSuccess)
                return pageResult.ToContract<long>();
            return pageResult.Result.Number;
        }

        [JsonCustomSerialization]
        public async Task<MessageContract<List<PageContract>>> GetPagesByBookId([NumberValidation] long bookId)
        {
            var pageResult = await new LogicBase<TranslatorContext, PageContract, PageEntity>().GetAll(x =>
                       x.Where(q => q.Catalog.BookId == bookId).Include(q => q.Audios));
            CacheLogic.FixPageBookIds(pageResult.Result);
            return pageResult;
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
                    PageNumber = CacheLogic.Pages[paragraph.PageId].Number,
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

        public async Task<MessageContract> UploadFile(StreamInfo<long> stream)
        {
            var verseResult = await new LogicBase<TranslatorContext, PageEntity, PageEntity>().Find(x =>
                        x.Include(q => q.Audios).Where(q => q.Id == stream.Data));
            if (!verseResult.IsSuccess)
                return verseResult.ToContract<MessageContract>();

            var bytes = await StreamHelper.ReadAllBytes(stream);
            if (verseResult.Result.Audios.Count == 0)
            {
                var audioEntity = new AudioEntity()
                {
                    Data = bytes,
                    PageId = stream.Data,
                    FileName = stream.FileName
                };
                var addAudioEntity = await new LogicBase<TranslatorContext, AudioEntity>().Add(audioEntity);
                if (!addAudioEntity.IsSuccess)
                    return addAudioEntity;
            }
            else
            {
                var audioEntity = verseResult.Result.Audios[0];
                audioEntity.FileName = stream.FileName;
                audioEntity.Data = bytes;
                var updateAudioEntity = await new LogicBase<TranslatorContext, AudioEntity, AudioEntity>().Update(audioEntity);
                if (!updateAudioEntity.IsSuccess)
                    return updateAudioEntity;
            }

            return true;
        }

        public async Task<ActionResult> DownloadFile(long pageId)
        {
            var context = OperationContext.Current;
            try
            {
                var verseResult = await new LogicBase<TranslatorContext, PageEntity, PageEntity>().Find(x =>
                        x.Include(q => q.Audios).Where(q => q.Id == pageId));
                var audio = verseResult.Result?.Audios?.FirstOrDefault();
                if (!verseResult.IsSuccess || audio == null)
                {
                    context.HttpClient.Status = System.Net.HttpStatusCode.NotFound;
                    return $"File Or Page Not found";
                }
                context.HttpClient.ResponseHeaders.Add("Content-Disposition", $"attachment; filename=\"{audio.FileName}\"");
                context.HttpClient.ResponseHeaders.Add("Content-Length", audio.Data.Length.ToString());
                context.HttpClient.ResponseHeaders.Add("Content-Type", "audio/mpeg");
                //context.HttpClient.ResponseHeaders.Add("Cache-Control", "max-age=604800");//604800 sec = 1 week
                //context.HttpClient.ResponseHeaders.Add("Date", fileInfo.LastUpdateDateTime.ToUniversalTime().ToString("R"));
                //context.HttpClient.ResponseHeaders.Add("ETag", $"{fileInfo.LastUpdateDateTime.Ticks}");

                //context.HttpClient.ResponseHeaders.Add("Last-Modified", fileInfo.LastUpdateDateTime.ToString());
                return new FileActionResult(new MemoryStream(audio.Data));
            }
            catch (Exception ex)
            {
                context.HttpClient.Status = System.Net.HttpStatusCode.InternalServerError;
                return new ActionResult(ex.ToString());
            }
        }

        public async Task<ActionResult> GetOfflineCache()
        {
            var context = OperationContext.Current;
            var result = CacheLogic.Pages.Values.ToList();
            return StreamHelper.GetOfflineCache(context, result, typeof(ObsoletePageService));
        }
    }
}