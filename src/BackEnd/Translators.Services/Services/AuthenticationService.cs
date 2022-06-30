using SignalGo.Server.Models;
using SignalGo.Shared.DataTypes;
using Translators.Contracts.Common;
using Translators.Contracts.Common.Authentications;
using Translators.Database.Contexts;
using Translators.Database.Entities.Authentications;
using Translators.Logics;

namespace Translators.Services
{
    [ServiceContract("Authentication", ServiceType.HttpService, InstanceType.SingleInstance)]
    [ServiceContract("Authentication", ServiceType.ServerService, InstanceType.SingleInstance)]
    public class AuthenticationService
    {
        public async Task<MessageContract<string>> Register(string userName)
        {
            var context = OperationContext.Current;
            var getOrCreateUser = await new LogicBase<TranslatorContext, UserEntity, UserEntity>().Find(query => query.Where(x => x.UserName == userName));
            if (!getOrCreateUser.IsSuccess)
                return getOrCreateUser.ToContract<string>();
            else if (getOrCreateUser.Result != null)
            {
                OperationContextBase.SetSetting(new UserContract()
                {
                    Key = getOrCreateUser.Result.UserSession.ToString(),
                    UserName = getOrCreateUser.Result.UserName,
                }, context);
                return getOrCreateUser.Result.UserSession.ToString();
            }
            var key = Guid.NewGuid();
            var user = await new LogicBase<TranslatorContext, UserEntity>().Add(new UserEntity()
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
            OperationContextBase.SetSetting(new UserContract()
            {
                Key = key.ToString(),
                UserName = userName,
            }, context);
            return key.ToString();
        }

        public async Task<MessageContract<string>> Login(Guid session)
        {
            var context = OperationContext.Current;
            var getOrCreateUser = await new LogicBase<TranslatorContext, UserEntity, UserEntity>().Find(query => query.Where(x => x.UserSession == session));
            if (!getOrCreateUser.IsSuccess)
                return getOrCreateUser.ToContract<string>();
            else if (getOrCreateUser.Result != null)
            {
                OperationContextBase.SetSetting(new UserContract()
                {
                    Key = getOrCreateUser.Result.UserSession.ToString(),
                    UserName = getOrCreateUser.Result.UserName,
                }, context);
                return getOrCreateUser.Result.UserSession.ToString();
            }
            return "اطلاعات جهتت ورود شما اشتباه است.";
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
