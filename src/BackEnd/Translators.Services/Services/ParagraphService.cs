﻿using SignalGo.Server.Models;
using SignalGo.Shared.DataTypes;
using Translators.Attributes;
using Translators.Contracts.Common;
using Translators.Contracts.Common.Authentications;
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
        public async Task<MessageContract> LinkParagraph([NumberValidation] long fromParagraphId, [NumberValidation] long toParagraphId)
        {
            var currentUser = OperationContext<UserContract>.CurrentSetting;
            if (currentUser == null || currentUser.Permissions == null)
                return ("شما وارد نشده اید یا ثبت نام نکرده اید!", FailedReasonType.SessionAccessDenied);
            else if (!currentUser.Permissions.Any(x => x == Contracts.Common.DataTypes.PermissionType.Admin))
                return ("شما دسترسی انجام اینکار را ندارید!", FailedReasonType.AccessDenied);

            if ((await new LogicBase<TranslatorContext, LinkParagraphEntity, LinkParagraphEntity>().Find(q => q.Where(x => x.FromParagraphId == fromParagraphId && x.ToParagraphId == toParagraphId))).Result != null)
                return ("این آیات قبلا به هم لینک شده بودند!", FailedReasonType.Dupplicate);
            else if ((await new LogicBase<TranslatorContext, LinkParagraphEntity, LinkParagraphEntity>().Find(q => q.Where(x => x.ToParagraphId == fromParagraphId && x.FromParagraphId == toParagraphId))).Result != null)
                return ("این آیات قبلا به هم لینک شده بودند!", FailedReasonType.Dupplicate);

            var result = await new LogicBase<TranslatorContext, LinkParagraphEntity>().Add(new LinkParagraphEntity()
            {
                LinkGroup = new LinkGroupEntity()
                {

                },
                FromParagraphId = fromParagraphId,
                ToParagraphId = toParagraphId
            });

            return result;
        }


        [JsonCustomSerialization]
        public async Task<MessageContract<List<SearchValueContract>>> GetLinkedParagraphs([NumberValidation] long paragraphId)
        {
            var links = await new LogicBase<TranslatorContext, LinkParagraphEntity, LinkParagraphEntity>().GetAll(q => q.Where(x => x.FromParagraphId == paragraphId || x.ToParagraphId == paragraphId));
            if (!links.IsSuccess)
                return links.ToContract<List<SearchValueContract>>();
            var paragraphIds = links.Result.SelectMany(x => new long[] { x.FromParagraphId, x.ToParagraphId }).Distinct().ToArray();
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
                });
            }

            return result;
        }
    }
}
