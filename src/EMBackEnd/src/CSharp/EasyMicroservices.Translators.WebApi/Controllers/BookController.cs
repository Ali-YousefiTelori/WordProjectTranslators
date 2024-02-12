using EasyMicroservices.ServiceContracts;
using EasyMicroservices.TranslatorsMicroservice;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Translators.Contracts.Common;
using Translators.Contracts.Common.OfflineCacheContracts;
using Translators.Database.Contexts;
using Translators.Database.Entities;
using Translators.Helpers;
using Translators.Logics;

namespace Translators.Services
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BookController : ControllerBase
    {
        AppUnitOfWork _appUnitOfWork;
        public BookController(AppUnitOfWork appUnitOfWork)
        {
            _appUnitOfWork = appUnitOfWork;
        }

        [HttpPost]
        public async Task<ListMessageContract<CategoryContract>> GetCategories()
        {
            using var _translatorContext = _appUnitOfWork.GetContext();
            return await new LogicBase<CategoryContract, CategoryEntity>(_translatorContext).GetAll(x => x.Include(q => q.Names).ThenInclude(n => n.Language).Include(q => q.Names).ThenInclude(n => n.Translator));
        }

        [HttpPost]
        public async Task<MessageContract<CategoryContract>> GetCategoryByBookId(long bookId)
        {
            using var _translatorContext = _appUnitOfWork.GetContext();
            return await new LogicBase<CategoryContract, CategoryEntity>(_translatorContext).Find(x => x.Include(q => q.Names).ThenInclude(n => n.Language).Include(q => q.Names).ThenInclude(n => n.Translator).Where(q => q.Books.Any(b => b.Id == bookId)));
        }

        [HttpPost]
        public async Task<ListMessageContract<BookContract>> GetBooks()
        {
            using var _translatorContext = _appUnitOfWork.GetContext();
            return await new LogicBase<BookContract, BookEntity>(_translatorContext).GetAll(x => x.Include(q => q.Names).ThenInclude(n => n.Language).Include(q => q.Names).ThenInclude(n => n.Translator).Where(x => !x.IsHidden));
        }

        [HttpPost]
        public async Task<MessageContract<BookContract>> GetBookById(long bookId)
        {
            using var _translatorContext = _appUnitOfWork.GetContext();
            return await new LogicBase<BookContract, BookEntity>(_translatorContext).Find(x => x.Include(q => q.Names).ThenInclude(n => n.Language).Include(q => q.Names).ThenInclude(n => n.Translator).Where(x => x.Id == bookId && !x.IsHidden));
        }

        [HttpPost]
        public async Task<ListMessageContract<BookContract>> FilterBooks(long categoryId)
        {
            using var _translatorContext = _appUnitOfWork.GetContext();
            return await new LogicBase<BookContract, BookEntity>(_translatorContext).GetAll(x => x.Include(q => q.Names).ThenInclude(n => n.Language).Include(q => q.Names).ThenInclude(n => n.Translator).Where(q => q.CategoryId == categoryId && !q.IsHidden));
        }

        [HttpPost]
        public async Task<FileStreamResult> GetOfflineCache()
        {
            return StreamHelper.GetOfflineCache(HttpContext, new BookServiceModelsContract()
            {
                Books = (await GetBooks()).Result,
                Categories = (await GetCategories()).Result
            }, typeof(BookController));
        }
    }
}
