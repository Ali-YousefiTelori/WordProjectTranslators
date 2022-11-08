using SignalGo.Server.Models;
using SignalGo.Shared.DataTypes;
using Translators.Attributes;
using Translators.Contracts.Common;
using Translators.Contracts.Common.Authentications;
using Translators.Contracts.Common.DataTypes;
using Translators.Database.Contexts;
using Translators.Database.Entities.UserPersonalization;
using Translators.Logics;
using Translators.Security;
using Translators.Validations;

namespace Translators.Services
{
    [ServiceContract("UserReading", ServiceType.HttpService, InstanceType.SingleInstance)]
    [ServiceContract("UserReading", ServiceType.ServerService, InstanceType.SingleInstance)]
    [AuthenticationSecurityPermission(PermissionType.EndUser)]
    public class UserReadingService
    {
        [JsonCustomSerialization]
        public async Task<MessageContract<List<UserReadingContract>>> GetUserReadings()
        {
            var currentUser = OperationContext<UserContract>.CurrentSetting;
            var readingsResult = await new LogicBase<TranslatorContext, UserReadingContract, ReadingEntity>().GetAll(x =>
                        x.Where(q => q.UserId == currentUser.UserId && !q.IsDeleted));
            return readingsResult;
        }

        public async Task<MessageContract> SyncUserReadings([NullOrEmptyValidation] List<UserReadingContract> readings)
        {
            var currentUser = OperationContext<UserContract>.CurrentSetting;
            var readingsResult = await new LogicBase<TranslatorContext, ReadingEntity>().GetAll(x =>
                        x.Where(q => q.UserId == currentUser.UserId && !q.IsDeleted));
            if (!readingsResult.IsSuccess)
                return readingsResult;
            if (readings.Count == 0)
            {
                foreach (var item in readingsResult.Result)
                {
                    item.IsDeleted = true;
                }
                return await new LogicBase<TranslatorContext, ReadingEntity>().UpdateAll(readingsResult.Result);
            }
            else
            {
                foreach (var item in readingsResult.Result)
                {
                    var find = readings.FirstOrDefault(x => x.Name == item.Name);
                    if (find == null)
                    {
                        item.IsDeleted = true;
                    }
                    else
                    {
                        item.Title = find.Title;
                        item.StartPageNumber = find.StartPageNumber;
                        item.BookId = find.BookId;
                        item.CatalogId = find.CatalogId;
                        item.CategoryId = find.CategoryId;
                        item.PageId = find.PageId;
                    }
                }

                foreach (var item in readings)
                {
                    if (!readingsResult.Result.Any(x => x.Name == item.Name))
                    {
                        var entity = item.Map<ReadingEntity>();
                        entity.UserId = currentUser.UserId;
                        readingsResult.Result.Add(entity);
                    }
                }

                var updateResult = await new LogicBase<TranslatorContext, ReadingEntity>().UpdateAll(readingsResult.Result);
                if (!updateResult.IsSuccess)
                    return updateResult;
            }
            return true;
        }
    }
}
