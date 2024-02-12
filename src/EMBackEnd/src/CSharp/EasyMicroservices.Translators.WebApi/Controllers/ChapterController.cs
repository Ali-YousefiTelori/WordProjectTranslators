using EasyMicroservices.ServiceContracts;
using EasyMicroservices.TranslatorsMicroservice;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Translators.Contracts.Common;
using Translators.Database.Entities;
using Translators.Helpers;
using Translators.Logics;

namespace Translators.Services
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ChapterController : ControllerBase
    {
        AppUnitOfWork _appUnitOfWork;
        public ChapterController(AppUnitOfWork appUnitOfWork)
        {
            _appUnitOfWork = appUnitOfWork;
        }
        [HttpPost]
        public async Task<ListMessageContract<CatalogContract>> FilterChapters(long bookId)
        {
            using var context = _appUnitOfWork.GetContext();
            return await new LogicBase<CatalogContract, CatalogEntity>(context).GetAll(x => x.Include(q => q.Names).ThenInclude(n => n.Language).Include(q => q.Names).ThenInclude(n => n.Translator).Where(q => q.BookId == bookId));
        }

        [HttpPost]
        public async Task<MessageContract<CatalogContract>> GetChapters(long chapterId)
        {
            using var context = _appUnitOfWork.GetContext();
            var chapterResult = await new LogicBase<CatalogContract, CatalogEntity>(context)
                .Find(x => x.Include(q => q.Names)
                .ThenInclude(n => n.Language)
                .Include(q => q.Names)
                .ThenInclude(n => n.Translator)
                .Where(q => q.Id == chapterId));
            if (!chapterResult.IsSuccess)
                return chapterResult;
            var bookResult = await new LogicBase<BookContract, BookEntity>(context)
                .Find(x => x.Include(q => q.Names)
                .ThenInclude(n => n.Language)
                .Include(q => q.Names)
                .ThenInclude(n => n.Translator)
                .Where(q => q.Id == chapterResult.Result.BookId));
            chapterResult.Result.BookNames = bookResult.Result.Names;
            return chapterResult;
        }

        [HttpPost]
        public async Task<FileStreamResult> GetOfflineCache()
        {
            using var context = _appUnitOfWork.GetContext();
            var result = await new LogicBase<CatalogContract, CatalogEntity>(context).GetAll(x => x.Include(q => q.Names).ThenInclude(n => n.Language).Include(q => q.Names).ThenInclude(n => n.Translator));
            foreach (var item in result.Result)
            {
                item.BookNames = (await _appUnitOfWork.GetCacheLogic().GetBook(item.BookId)).Names;
            }
            return StreamHelper.GetOfflineCache(HttpContext, result.Result, typeof(ChapterController));
        }
    }
}
