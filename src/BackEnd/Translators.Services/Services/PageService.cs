using Microsoft.EntityFrameworkCore;
using SignalGo.Shared.DataTypes;
using Translators.Contracts.Common;
using Translators.Database.Contexts;
using Translators.Database.Entities;
using Translators.Logics;

namespace Translators.Services
{
    [ServiceContract("Page", ServiceType.HttpService, InstanceType.SingleInstance)]
    [ServiceContract("Page", ServiceType.ServerService, InstanceType.SingleInstance)]
    public class PageService
    {
        public async Task<MessageContract<List<PageContract>>> GetPage(long pageNumber)
        {
            return await new LogicBase<TranslatorContext, PageContract, PageEntity>().GetAll(x =>
                         x.Include(q => q.Paragraphs).ThenInclude(p => p.Words).ThenInclude(w => w.Value)
                        .Where(q => q.Number == pageNumber));
        }
    }
}