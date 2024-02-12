using EasyMicroservices.ServiceContracts;
using EasyMicroservices.TranslatorsMicroservice;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Translators.Contracts.Common;
using Translators.Contracts.Requests;
using Translators.Database.Entities;
using Translators.Helpers;
using Translators.Logics;

namespace Translators.Services
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ObsoletePageController : ControllerBase
    {
        AppUnitOfWork _appUnitOfWork;
        public ObsoletePageController(AppUnitOfWork appUnitOfWork)
        {
            _appUnitOfWork = appUnitOfWork;
        }

        [HttpPost]
        public async Task<MessageContract<long>> GetPageNumberByVerseNumber(long verseNumber, long catalogId)
        {
            using var context = _appUnitOfWork.GetContext();
            var verseResult = await new LogicBase<ParagraphContract, ParagraphEntity>(context).Find(x =>
                        x.Where(q => q.Number == verseNumber && q.Page.CatalogId == catalogId));
            if (!verseResult.IsSuccess)
                return verseResult.ToContract<long>();
            var pageResult = await new LogicBase<PageContract, PageEntity>(context).Find(x =>
                       x.Where(q => q.Id == verseResult.Result.PageId));
            if (!pageResult.IsSuccess)
                return pageResult.ToContract<long>();
            return pageResult.Result.Number;
        }

        [HttpPost]
        public async Task<ListMessageContract<PageContract>> GetPagesByBookId(long bookId)
        {
            using var context = _appUnitOfWork.GetContext();
            var pageResult = await new LogicBase<PageContract, PageEntity>(context).GetAll(x =>
                       x.Where(q => q.Catalog.BookId == bookId).Include(q => q.Audios));
            await _appUnitOfWork.GetCacheLogic().FixPageBookIds(pageResult.Result);
            return pageResult;
        }

        [HttpPost]
        public async Task<ListMessageContract<SearchValueContract>> Search(AdvancedSearchFilterRequestContract advancedSearchFilterRequest)
        {
            return FailedReasonType.Nothing;
            //var cacheLogic = _appUnitOfWork.GetCacheLogic();
            //advancedSearchFilterRequest.Search = TextHelper.FixArabicForSearch(advancedSearchFilterRequest.Search).Trim();
            //if (string.IsNullOrEmpty(advancedSearchFilterRequest.Search))
            //    return (FailedReasonType.Empty, "لطفا متن جستجو را پر کنید!");
            //string[] searchKeys;
            //if (advancedSearchFilterRequest.DoFullWordsSearch)
            //    searchKeys = new string[] { advancedSearchFilterRequest.Search.Replace("  ", " ").Trim() };
            //else
            //    searchKeys = advancedSearchFilterRequest.Search.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
            //if (searchKeys.Length == 0)
            //    return (FailedReasonType.Empty, "لطفا متن جستجو را پر کنید!");
            //var bookIds = advancedSearchFilterRequest.BookIds.ToArray();
            //var query = cacheLogic.Paragraphs.Values.AsQueryable();
            ////if (advancedSearchFilterRequest.SkipSearchInTranslates)
            ////{
            ////    query = query.Where(x => x.TranslatorId == null);
            ////}

            ////if (advancedSearchFilterRequest.SkipSearchInMain)
            ////{
            ////    query = query.Where(x => x.TranslatorId > 0);
            ////}

            //if (bookIds.Length > 0)
            //{
            //    //query = query.Where(x => bookIds.Contains(cacheLogic.Catalogs[cacheLogic.Paragraphs[x.Id].CatalogId].BookId));
            //}

            //if (advancedSearchFilterRequest.DoFullWordsSearch)
            //    query = query.Where(x => (!advancedSearchFilterRequest.SkipSearchInMain && x.GetMainSearchValue().Contains(searchKeys[0])) || (!advancedSearchFilterRequest.SkipSearchInTranslates && x.GetTranslatedSearchValue().Contains(searchKeys[0])));
            //else
            //    query = query.Where(x => searchKeys.All(key => x.Words.Any(w => w.Values.Any(v => CanSearch(advancedSearchFilterRequest.SkipSearchInTranslates, advancedSearchFilterRequest.SkipSearchInMain, key, v)))));


            //var paragraphs = query.ToList();

            //List<SearchValueContract> result = new List<SearchValueContract>();
            //foreach (var paragraph in paragraphs)
            //{
            //    //var paragraph = CacheLogic.Paragraphs[CacheLogic.Words[word.WordValueId.Value].ParagraphId];
            //    var catalog = await cacheLogic.GetCatalog(paragraph.CatalogId);
            //    result.Add(new SearchValueContract()
            //    {
            //        HasLink = paragraph.HasLink,
            //        Number = paragraph.Number,
            //        //Value = word,
            //        ParagraphWords = paragraph.Words,
            //        ParagraphId = paragraph.Id,
            //        PageId = paragraph.PageId,
            //        PageNumber = (await cacheLogic.GetPage(paragraph.PageId)).Number,
            //        CatalogId = paragraph.CatalogId,
            //        CatalogNames = catalog.Names,
            //        BookId = catalog.BookId,
            //        BookNames = (await cacheLogic.GetBook(catalog.BookId)).Names,
            //    });
            //}
            //return result;
        }
        bool CanSearch(bool skipSearchInTranslates, bool skipSearchInMain, string key, ValueContract value)
        {
            if (!value.IsMain && skipSearchInTranslates)
                return false;
            if (value.IsMain && skipSearchInMain)
                return false;
            return value.SearchValue.Contains(key) || value.Value.Contains(key);
        }

        [HttpPost]
        public async Task<MessageContract> UploadFile(long id, string fileName, Stream stream)
        {
            using var context = _appUnitOfWork.GetContext();
            var verseResult = await new LogicBase<PageEntity, PageEntity>(context).Find(x =>
                        x.Include(q => q.Audios).Where(q => q.Id == id));
            if (!verseResult.IsSuccess)
                return verseResult.ToContract<MessageContract>();

            var bytes = await StreamHelper.ReadAllBytes(stream);
            if (verseResult.Result.Audios.Count == 0)
            {
                var audioEntity = new AudioEntity()
                {
                    Data = bytes,
                    PageId = id,
                    FileName = fileName
                };
                var addAudioEntity = await new LogicBase<AudioEntity>(context).Add(audioEntity);
                if (!addAudioEntity.IsSuccess)
                    return addAudioEntity;
            }
            else
            {
                var audioEntity = verseResult.Result.Audios[0];
                audioEntity.FileName = fileName;
                audioEntity.Data = bytes;
                var updateAudioEntity = await new LogicBase<AudioEntity>(context).Update(audioEntity);
                if (!updateAudioEntity.IsSuccess)
                    return updateAudioEntity;
            }

            return true;
        }

        [HttpPost]
        public async Task<FileStreamResult> DownloadFile(long pageId)
        {
            using var context = _appUnitOfWork.GetContext();
            var verseResult = await new LogicBase<PageEntity>(context).Find(x =>
                    x.Include(q => q.Audios).Where(q => q.Id == pageId));
            var audio = verseResult.Result?.Audios?.FirstOrDefault();
            if (!verseResult.IsSuccess || audio == null)
            {
                HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
                throw new Exception("file not found!");
            }
            HttpContext.Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{audio.FileName}\"");
            HttpContext.Response.Headers.Add("Content-Length", audio.Data.Length.ToString());
            HttpContext.Response.Headers.Add("Content-Type", "audio/mpeg");
            //context.HttpClient.ResponseHeaders.Add("Cache-Control", "max-age=604800");//604800 sec = 1 week
            //context.HttpClient.ResponseHeaders.Add("Date", fileInfo.LastUpdateDateTime.ToUniversalTime().ToString("R"));
            //context.HttpClient.ResponseHeaders.Add("ETag", $"{fileInfo.LastUpdateDateTime.Ticks}");

            //context.HttpClient.ResponseHeaders.Add("Last-Modified", fileInfo.LastUpdateDateTime.ToString());
            return new FileStreamResult(new MemoryStream(audio.Data), "audio/mpeg");
        }

        [HttpPost]
        public async Task<ActionResult> GetOfflineCache()
        {
            var cacheLogic = _appUnitOfWork.GetCacheLogic();
            var result = await cacheLogic.GetPages();
            return StreamHelper.GetOfflineCache(HttpContext, result, typeof(ObsoletePageController));
        }
    }
}