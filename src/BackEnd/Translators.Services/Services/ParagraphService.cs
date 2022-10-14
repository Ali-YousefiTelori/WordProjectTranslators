using SignalGo.Server.Models;
using SignalGo.Shared.DataTypes;
using Translators.Attributes;
using Translators.Contracts.Common;
using Translators.Contracts.Common.Authentications;
using Translators.Contracts.Requests;
using Translators.Database.Contexts;
using Translators.Database.Entities;
using Translators.Logics;
using Translators.Validations;

namespace Translators.Services
{
    [ServiceContract("Paragraph", ServiceType.HttpService, InstanceType.SingleInstance)]
    [ServiceContract("Paragraph", ServiceType.ServerService, InstanceType.SingleInstance)]
    public class ParagraphService
    {
        [JsonCustomSerialization]
        public async Task<MessageContract<List<LinkGroupContract>>> GetLinkGroups()
        {
            return await new LogicBase<TranslatorContext, LinkGroupContract, LinkGroupEntity>().GetAll();
        }

        [JsonCustomSerialization]
        public async Task<MessageContract> LinkParagraph([NumberValidation] LinkParagraphRequestContract linkParagraphRequest)
        {
            var currentUser = OperationContext<UserContract>.CurrentSetting;
            if (currentUser == null || currentUser.Permissions == null)
                return ("شما وارد نشده اید یا ثبت نام نکرده اید!", FailedReasonType.SessionAccessDenied);
            else if (!currentUser.Permissions.Any(x => x == Contracts.Common.DataTypes.PermissionType.Admin))
                return ("شما دسترسی انجام اینکار را ندارید!", FailedReasonType.AccessDenied);
            else if (linkParagraphRequest.FromParagraphIds.Count == 0 || linkParagraphRequest.ToParagraphIds.Count == 0)
                return ("هیچ آیه‌ای برای لینک وجود ندارد!", FailedReasonType.Empty);

            //if ((await new LogicBase<TranslatorContext, LinkParagraphEntity, LinkParagraphEntity>().Find(q => q.Where(x => x.FromParagraphId == linkParagraphRequest.FromParagraphId && x.ToParagraphId == linkParagraphRequest.ToParagraphId && x.LinkGroup.Title == linkParagraphRequest.Title))).Result != null)
            //    return ("این آیات قبلا به هم لینک شده بودند!", FailedReasonType.Dupplicate);
            //else if ((await new LogicBase<TranslatorContext, LinkParagraphEntity, LinkParagraphEntity>().Find(q => q.Where(x => x.ToParagraphId == linkParagraphRequest.FromParagraphId && x.FromParagraphId == linkParagraphRequest.ToParagraphId && x.LinkGroup.Title == linkParagraphRequest.Title))).Result != null)
            //    return ("این آیات قبلا به هم لینک شده بودند!", FailedReasonType.Dupplicate);

            var findGroup = await new LogicBase<TranslatorContext, LinkGroupEntity, LinkGroupEntity>().Find(q => q.Where(x => x.Title == linkParagraphRequest.Title));
            if (!findGroup.HasResult())
            {
                var addGroup = await new LogicBase<TranslatorContext, LinkGroupEntity>().Add(new LinkGroupEntity()
                {
                    Title = linkParagraphRequest.Title
                });
                if (!addGroup.IsSuccess)
                    return addGroup;
                findGroup = addGroup;
            }
            var oldLinks = await new LogicBase<TranslatorContext, LinkParagraphEntity, LinkParagraphEntity>().GetAll(q => q.Where(x => x.LinkGroupId == findGroup.Result.Id));
            if (!oldLinks.IsSuccess)
                return oldLinks;
            var paragraphIds = linkParagraphRequest.ToParagraphIds.Union(linkParagraphRequest.FromParagraphIds).Union(oldLinks.Result.Select(r => r.ParagraphId)).ToList();
            var result = await new LogicBase<TranslatorContext, LinkParagraphEntity>().AddRange(paragraphIds.Select(id => new LinkParagraphEntity
            {
                LinkGroupId = findGroup.Result.Id,
                UserId = currentUser.UserId,
                ParagraphId = id,
            }).ToList());

            return result;
        }

        [JsonCustomSerialization]
        public async Task<MessageContract<List<LinkGroupContract>>> GetLinkedParagraphsGroups([NumberValidation] long paragraphId)
        {
            var links = await new LogicBase<TranslatorContext, LinkParagraphEntity, LinkParagraphEntity>().GetAll(q => q.Where(x => x.ParagraphId == paragraphId));
            if (!links.IsSuccess)
                return links.ToContract<List<LinkGroupContract>>();
            var groupIds = links.Result.GroupBy(x => x.LinkGroupId).Select(x => x.Key).ToList();
            return await new LogicBase<TranslatorContext, LinkGroupContract, LinkGroupEntity>().GetAll(q => q.Where(x => groupIds.Contains(x.Id)));
        }

        [JsonCustomSerialization]
        public async Task<MessageContract<List<SearchValueContract>>> GetLinkedParagraphs([NumberValidation] long groupId)
        {
            var links = await new LogicBase<TranslatorContext, LinkParagraphEntity, LinkParagraphEntity>().GetAll(q => q.Where(x => x.LinkGroupId == groupId));
            if (!links.IsSuccess)
                return links.ToContract<List<SearchValueContract>>();
            var paragraphIds = links.Result.Select(x => x.ParagraphId).Distinct().ToArray();
            List<SearchValueContract> result = new List<SearchValueContract>();
            foreach (var pId in paragraphIds)
            {
                var paragraph = CacheLogic.Paragraphs[pId];
                var catalog = CacheLogic.Catalogs[paragraph.CatalogId];
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
                    BookNames = CacheLogic.Books[catalog.BookId].Names,
                    PageNumber = CacheLogic.Pages[paragraph.PageId].Number
                });
            }

            return result;
        }
    }
}
