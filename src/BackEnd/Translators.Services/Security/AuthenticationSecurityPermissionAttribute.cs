using SignalGo.Server.DataTypes;
using SignalGo.Server.Models;
using System.Reflection;
using Translators.Contracts.Common;
using Translators.Contracts.Common.Authentications;
using Translators.Contracts.Common.DataTypes;

namespace Translators.Security
{
    public class AuthenticationSecurityPermissionAttribute : SecurityContractAsyncAttribute
    {
        PermissionType[] permissionTypes;
        public AuthenticationSecurityPermissionAttribute(params PermissionType[] permissions)
        {
            permissionTypes = permissions;
        }

        public override Task<bool> CheckPermissionAsync(int taskId, ClientInfo client, object service, MethodInfo method, List<object> parameters)
        {
            var currentUser = OperationContext<UserContract>.CurrentSetting;
            if (currentUser == null || currentUser.Permissions == null)
                return Task.FromResult(false);
            else if (!currentUser.Permissions.Any(x => permissionTypes.Contains(x)))
                return Task.FromResult(false);
            return Task.FromResult(true);
        }

        public override Task<object> GetValueWhenDenyPermissionAsync(int taskId, ClientInfo client, object service, MethodInfo method, List<object> parameters)
        {
            MessageContract result = default;
            var currentUser = OperationContext<UserContract>.CurrentSetting;
            if (currentUser == null || currentUser.Permissions == null)
                result = (FailedReasonType.SessionAccessDenied, "شما وارد نشده اید یا ثبت نام نکرده اید!");
            else
                result = (FailedReasonType.AccessDenied, "شما دسترسی انجام اینکار را ندارید!");
            return Task.FromResult((object)result);
        }
    }
}
