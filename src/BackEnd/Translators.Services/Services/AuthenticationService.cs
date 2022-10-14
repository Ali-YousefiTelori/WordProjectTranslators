using Microsoft.EntityFrameworkCore;
using SignalGo.Server.Models;
using SignalGo.Shared.DataTypes;
using SignalGo.Shared.Http;
using Translators.Contracts.Common;
using Translators.Contracts.Common.Authentications;
using Translators.Contracts.Responses;
using Translators.Database.Contexts;
using Translators.Database.Entities;
using Translators.Database.Entities.Authentications;
using Translators.Logics;

namespace Translators.Services
{
    [ServiceContract("Authentication", ServiceType.HttpService, InstanceType.SingleInstance)]
    [ServiceContract("Authentication", ServiceType.ServerService, InstanceType.SingleInstance)]
    public class AuthenticationService
    {
        public async Task<MessageContract<UserContract>> Register(string userName)
        {
            var context = OperationContext.Current;
            var getOrCreateUser = await new LogicBase<TranslatorContext, UserEntity, UserEntity>().Find(query => query.Where(x => x.UserName == userName));
            if (!getOrCreateUser.IsSuccess)
                return getOrCreateUser.ToContract<UserContract>();
            else if (getOrCreateUser.Result != null)
            {
                return ("نام کاربری تکراری است!", "");
            }
            var key = Guid.NewGuid();
            var userResult = await new LogicBase<TranslatorContext, UserEntity>().Add(new UserEntity()
            {
                IsConfirmed = false,
                UserName = userName,
                UserSession = key,
                UserPermissions = new List<UserPermissionEntity>()
                {
                    new UserPermissionEntity()
                    {
                         PermissionType = Contracts.Common.DataTypes.PermissionType.EndUser
                    }
                }
            });
            if (!userResult.IsSuccess)
                return userResult.ToContract<UserContract>();
            var user = new UserContract()
            {
                UserId = userResult.Result.Id,
                Key = key.ToString(),
                UserName = userName,
                Permissions = userResult.Result.UserPermissions.Select(x => x.PermissionType).ToList()
            };
            OperationContextBase.SetSetting(user, context);
            return user;
        }

        public async Task<MessageContract<UserContract>> Login(Guid session)
        {
            var context = OperationContext.Current;
            var getOrCreateUser = await new LogicBase<TranslatorContext, UserEntity, UserEntity>().Find(query => query.Include(x => x.UserPermissions).Where(x => x.UserSession == session));
            if (!getOrCreateUser.IsSuccess)
                return getOrCreateUser.ToContract<UserContract>();
            else if (getOrCreateUser.Result != null)
            {
                var user = new UserContract()
                {
                    UserId = getOrCreateUser.Result.Id,
                    Key = getOrCreateUser.Result.UserSession.ToString(),
                    UserName = getOrCreateUser.Result.UserName,
                    Permissions = getOrCreateUser.Result.UserPermissions.Select(x => x.PermissionType).ToList()
                };
                OperationContextBase.SetSetting(user, context);
                return user;
            }
            return ("اطلاعات شما جهت ورود اشتباه وارد شده است.", "");
        }

        //public async Task<MessageContract<string>> LoginOrRegister(string userName)
        //{
        //    string code = AuthenticationLogic.GetRandomCode();
        //    var key = Guid.NewGuid();
        //    var context = OperationContext<UserContract>.CurrentSetting = new UserContract()
        //    {
        //        Key = key.ToString(),
        //        LoginCode = code,
        //        UserName = userName,
        //    };

        //    var getOrCreateUser = await new LogicBase<TranslatorContext, UserEntity, UserEntity>().Find(query => query.Where(x => x.UserName == userName));
        //    if (getOrCreateUser == null)
        //    {
        //        await new LogicBase<TranslatorContext, UserEntity, UserEntity>().Add(new UserEntity()
        //        {
        //            IsConfirmed = false,
        //            UserName = userName,
        //            UserSession = key,
        //            UserPermissions = new List<UserPermissionEntity>()
        //            {
        //                new UserPermissionEntity()
        //                {
        //                     PermissionType = Contracts.Common.DataTypes.PermissionType.EndUser
        //                }
        //            }
        //        });
        //    }
        //    var result = await AuthenticationLogic.SendSms(userName, code);
        //    if (!result.IsSuccess)
        //        return result.ToContract<string>();
        //    return context.Key;
        //}
    }
}
