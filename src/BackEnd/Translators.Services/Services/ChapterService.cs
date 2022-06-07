using Microsoft.EntityFrameworkCore;
using SignalGo.Shared.DataTypes;
using Translators.Contracts.Common;
using Translators.Database.Contexts;
using Translators.Database.Entities;
using Translators.Logics;

namespace Translators.Services
{
    [ServiceContract("Chapter", ServiceType.HttpService, InstanceType.SingleInstance)]
    [ServiceContract("Chapter", ServiceType.ServerService, InstanceType.SingleInstance)]
    public class ChapterService
    {
        public async Task<MessageContract<List<CatalogContract>>> FilterChapters(long bookId)
        {
            return await new LogicBase<TranslatorContext, CatalogContract, CatalogEntity>().GetAll(x => x.Include(q => q.Name).Where(q => q.BookId == bookId));
        }
    }
}
