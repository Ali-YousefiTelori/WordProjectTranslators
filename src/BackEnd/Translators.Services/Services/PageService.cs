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
        public async Task<MessageContract<PageContract>> GetPage(long pageNumber)
        {
            var result = await new LogicBase<TranslatorContext, PageContract, PageEntity>().Find(x =>
                         x.Include(q => q.Paragraphs).ThenInclude(p => p.Words).ThenInclude(w => w.Values).ThenInclude(n => n.Language)
                         .Include(q => q.Paragraphs).ThenInclude(p => p.Words).ThenInclude(w => w.Values).ThenInclude(n => n.Translator)
                        .Where(q => q.Number == pageNumber));
            var catalogIds = result.Result.Paragraphs.GroupBy(x => x.CatalogId).Select(x => x.Key).ToArray();
            var catalogs = await new LogicBase<TranslatorContext, CatalogContract, CatalogEntity>().GetAll(x => x.Include(c => c.Names).ThenInclude(c => c.Language).Where(i => catalogIds.Contains(i.Id)));
            foreach (var paragraph in result.Result.Paragraphs)
            {
                result.Result.CatalogNames = result.Result.Paragraphs.SelectMany(p => catalogs.Result.FirstOrDefault(x => x.Id == p.CatalogId)?.Names).Distinct().ToList();
            }
            return result;
        }
    }
}