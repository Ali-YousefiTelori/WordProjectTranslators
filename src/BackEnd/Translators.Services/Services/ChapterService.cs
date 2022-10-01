using Microsoft.EntityFrameworkCore;
using SignalGo.Server.Models;
using SignalGo.Shared.DataTypes;
using SignalGo.Shared.Http;
using System.Net;
using Translators.Attributes;
using Translators.Contracts.Common;
using Translators.Contracts.Common.OfflineCacheContracts;
using Translators.Database.Contexts;
using Translators.Database.Entities;
using Translators.Helpers;
using Translators.Logics;
using Translators.Validations;

namespace Translators.Services
{
    [ServiceContract("Chapter", ServiceType.HttpService, InstanceType.SingleInstance)]
    [ServiceContract("Chapter", ServiceType.ServerService, InstanceType.SingleInstance)]
    public class ChapterService
    {
        [JsonCustomSerialization]
        public async Task<MessageContract<List<CatalogContract>>> FilterChapters([NumberValidation] long bookId)
        {
            return await new LogicBase<TranslatorContext, CatalogContract, CatalogEntity>().GetAll(x => x.Include(q => q.Names).ThenInclude(n => n.Language).Include(q => q.Names).ThenInclude(n => n.Translator).Where(q => q.BookId == bookId));
        }

        [JsonCustomSerialization]
        public async Task<MessageContract<CatalogContract>> GetChapters([NumberValidation] long chapterId)
        {
            var chapterResult =  await new LogicBase<TranslatorContext, CatalogContract, CatalogEntity>().Find(x => x.Include(q => q.Names).ThenInclude(n => n.Language).Include(q => q.Names).ThenInclude(n => n.Translator).Where(q => q.Id == chapterId));
            if (!chapterResult.IsSuccess)
                return chapterResult;
            var bookResult = await new LogicBase<TranslatorContext, BookContract, BookEntity>().Find(x => x.Include(q => q.Names).ThenInclude(n => n.Language).Include(q => q.Names).ThenInclude(n => n.Translator).Where(q => q.Id == chapterResult.Result.BookId));
            chapterResult.Result.BookNames = bookResult.Result.Names;
            return chapterResult;
        }

        public async Task<ActionResult> GetOfflineCache()
        {
            var context = OperationContext.Current;
            var result = await new LogicBase<TranslatorContext, CatalogContract, CatalogEntity>().GetAll(x => x.Include(q => q.Names).ThenInclude(n => n.Language).Include(q => q.Names).ThenInclude(n => n.Translator));
            foreach (var item in result.Result)
            {
                item.BookNames = CacheLogic.Books[item.BookId].Names;
            }
            return StreamHelper.GetOfflineCache(context, result.Result, typeof(ChapterService));
        }
    }
}
