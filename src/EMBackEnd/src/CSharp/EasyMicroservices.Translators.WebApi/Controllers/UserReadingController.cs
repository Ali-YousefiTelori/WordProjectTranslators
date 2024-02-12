using EasyMicroservices.ServiceContracts;
using EasyMicroservices.TranslatorsMicroservice;
using EasyMicroservices.TranslatorsMicroservice.Helpers;
using Microsoft.AspNetCore.Mvc;
using Translators.Contracts.Common;
using Translators.Database.Entities.UserPersonalization;
using Translators.Logics;
using Translators.Models;

namespace Translators.Services
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [TranslatorsSecurity("EndUser")]
    public class UserReadingController : ControllerBase
    {
        AppUnitOfWork _appUnitOfWork;
        public UserReadingController(AppUnitOfWork appUnitOfWork)
        {
            _appUnitOfWork = appUnitOfWork;
        }


        [HttpPost]
        public async Task<ListMessageContract<UserReadingContract>> GetUserReadings()
        {
            using var context = _appUnitOfWork.GetContext();
            CurrentUser currentUser = _appUnitOfWork.GetCurrentUser();
            var readingsResult = await new LogicBase<UserReadingContract, ReadingEntity>(context).GetAll(x =>
                        x.Where(q => q.UserId == currentUser.UserId && !q.IsDeleted));
            return readingsResult;
        }

        [HttpPost]
        public async Task<MessageContract> SyncUserReadings(List<UserReadingContract> readings)
        {
            using var context = _appUnitOfWork.GetContext();
            CurrentUser currentUser = _appUnitOfWork.GetCurrentUser();
            var readingsResult = await new LogicBase<ReadingEntity>(context).GetAll(x =>
                        x.Where(q => q.UserId == currentUser.UserId && !q.IsDeleted));
            if (!readingsResult.IsSuccess)
                return readingsResult;
            if (readings.Count == 0)
            {
                foreach (var item in readingsResult.Result)
                {
                    item.IsDeleted = true;
                }
                return await new LogicBase<ReadingEntity>(context).UpdateAll(readingsResult.Result);
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

                var updateResult = await new LogicBase<ReadingEntity>(context).UpdateAll(readingsResult.Result);
                if (!updateResult.IsSuccess)
                    return updateResult;
            }
            return true;
        }
    }
}
