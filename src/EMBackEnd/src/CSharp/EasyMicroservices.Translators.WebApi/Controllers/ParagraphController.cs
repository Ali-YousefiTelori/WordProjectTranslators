using EasyMicroservices.ServiceContracts;
using EasyMicroservices.TranslatorsMicroservice;
using EasyMicroservices.TranslatorsMicroservice.Helpers;
using Microsoft.AspNetCore.Mvc;
using Translators.Contracts.Common;
using Translators.Contracts.Requests;
using Translators.Database.Entities;
using Translators.Logics;

namespace Translators.Services
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ParagraphController : ControllerBase
    {
        AppUnitOfWork _appUnitOfWork;
        public ParagraphController(AppUnitOfWork appUnitOfWork)
        {
            _appUnitOfWork = appUnitOfWork;
        }

        [HttpPost]
        public async Task<ListMessageContract<LinkGroupContract>> GetLinkGroups()
        {
            using var context = _appUnitOfWork.GetContext();
            return await new LogicBase<LinkGroupContract, LinkGroupEntity>(context).GetAll();
        }

        [TranslatorsSecurity("Admin")]
        [HttpPost]
        public async Task<MessageContract> LinkParagraph(LinkParagraphRequestContract linkParagraphRequest)
        {
            return FailedReasonType.NotFound;
            //var currentUser = OperationContext<UserContract>.CurrentSetting;
            //if (linkParagraphRequest.FromParagraphIds.Count == 0 || linkParagraphRequest.ToParagraphIds.Count == 0)
            //    return (FailedReasonType.Empty, "هیچ آیه‌ای برای لینک وجود ندارد!");
            //using var context = _appUnitOfWork.GetContext();

            ////if ((await new LogicBase<TranslatorContext, LinkParagraphEntity, LinkParagraphEntity>().Find(q => q.Where(x => x.FromParagraphId == linkParagraphRequest.FromParagraphId && x.ToParagraphId == linkParagraphRequest.ToParagraphId && x.LinkGroup.Title == linkParagraphRequest.Title))).Result != null)
            ////    return ("این آیات قبلا به هم لینک شده بودند!", FailedReasonType.Dupplicate);
            ////else if ((await new LogicBase<TranslatorContext, LinkParagraphEntity, LinkParagraphEntity>().Find(q => q.Where(x => x.ToParagraphId == linkParagraphRequest.FromParagraphId && x.FromParagraphId == linkParagraphRequest.ToParagraphId && x.LinkGroup.Title == linkParagraphRequest.Title))).Result != null)
            ////    return ("این آیات قبلا به هم لینک شده بودند!", FailedReasonType.Dupplicate);

            //var findGroup = await new LogicBase<LinkGroupEntity, LinkGroupEntity>(context).Find(q => q.Where(x => x.Title == linkParagraphRequest.Title));
            //if (!findGroup.HasResult())
            //{
            //    var addGroup = await new LogicBase<LinkGroupEntity>(context).Add(new LinkGroupEntity()
            //    {
            //        Title = linkParagraphRequest.Title
            //    });
            //    if (!addGroup.IsSuccess)
            //        return addGroup;
            //    findGroup = addGroup;
            //}
            //var oldLinks = await new LogicBase<LinkParagraphEntity>(context).GetAll(q => q.Where(x => x.LinkGroupId == findGroup.Result.Id));
            //if (!oldLinks.IsSuccess)
            //    return oldLinks;
            //var paragraphIds = linkParagraphRequest.ToParagraphIds.Concat(linkParagraphRequest.FromParagraphIds).Except(oldLinks.Result.Select(r => r.ParagraphId)).Distinct().ToList();
            //var result = await new LogicBase<LinkParagraphEntity>(context).AddRange(paragraphIds.Select(id => new LinkParagraphEntity
            //{
            //    LinkGroupId = findGroup.Result.Id,
            //    UserId = currentUser.UserId,
            //    ParagraphId = id,
            //}).ToList());

            //return result;
        }

        [HttpPost]
        public async Task<ListMessageContract<LinkGroupContract>> GetLinkedParagraphsGroups(long paragraphId)
        {
            using var context = _appUnitOfWork.GetContext();
            var links = await new LogicBase<LinkParagraphEntity>(context).GetAll(q => q.Where(x => x.ParagraphId == paragraphId));
            if (!links.IsSuccess)
                return links.ToListContract<LinkGroupContract>();
            var groupIds = links.Result.GroupBy(x => x.LinkGroupId).Select(x => x.Key).ToList();
            return await new LogicBase<LinkGroupContract, LinkGroupEntity>(context).GetAll(q => q.Where(x => groupIds.Contains(x.Id)));
        }

        [HttpPost]
        public async Task<ListMessageContract<SearchValueContract>> GetLinkedParagraphs(long groupId)
        {
            using var context = _appUnitOfWork.GetContext();
            var cacheLogic = _appUnitOfWork.GetCacheLogic();
            var links = await new LogicBase<LinkParagraphEntity>(context).GetAll(q => q.Where(x => x.LinkGroupId == groupId));
            if (!links.IsSuccess)
                return links.ToListContract<SearchValueContract>();
            var paragraphIds = links.Result.Select(x => x.ParagraphId).Distinct().ToArray();
            List<SearchValueContract> result = new List<SearchValueContract>();
            foreach (var pId in paragraphIds)
            {
                var paragraph = await cacheLogic.GetParagraph(pId);
                var catalog = await cacheLogic.GetCatalog(paragraph.CatalogId);
                result.Add(new SearchValueContract()
                {
                    HasLink = paragraph.HasLink,
                    Number = paragraph.Number,
                    ParagraphWords = paragraph.Words,
                    ParagraphId = paragraph.Id,
                    PageId = paragraph.PageId,
                    CatalogId = paragraph.CatalogId,
                    CatalogNames = catalog.Names,
                    BookId = catalog.BookId,
                    BookNames = (await cacheLogic.GetBook(catalog.BookId)).Names,
                    PageNumber = (await cacheLogic.GetPage(paragraph.PageId)).Number
                });
            }

            return result;
        }
    }
}
