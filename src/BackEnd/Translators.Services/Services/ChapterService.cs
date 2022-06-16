using Microsoft.EntityFrameworkCore;
using SignalGo.Shared.DataTypes;
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
        public async Task<MessageContract<List<CatalogContract>>> FilterChapters([NumberValidation] long bookId)
        {
            return await new LogicBase<TranslatorContext, CatalogContract, CatalogEntity>().GetAll(x => x.Include(q => q.Names).ThenInclude(n => n.Language).Include(q => q.Names).ThenInclude(n => n.Translator).Where(q => q.BookId == bookId));
        }
    }
}
