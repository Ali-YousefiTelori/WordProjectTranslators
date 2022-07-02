using Microsoft.EntityFrameworkCore;
using SignalGo.Shared.DataTypes;
using Translators.Attributes;
using Translators.Contracts.Common;
using Translators.Database.Contexts;
using Translators.Database.Entities;
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
    }
}
