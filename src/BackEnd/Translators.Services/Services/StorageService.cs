using SignalGo.Server.Models;
using SignalGo.Shared.DataTypes;
using SignalGo.Shared.Http;
using SignalGo.Shared.Models;
using Translators.Contracts.Common;
using Translators.Logics;

namespace Translators.Services
{
    [ServiceContract("Storage", ServiceType.HttpService, InstanceType.SingleInstance)]
    public class StorageService
    {
        public Task<ActionResult> DownloadFile(long fileId, string password)
        {
            var context = OperationContext.Current;
            return Task.FromResult(StorageLogic.GetFile(context, fileId, password));
        }

        //[AuthenticationSecurityPermission(PermissionType.Admin)]
        public async Task<MessageContract<AudioFileContract>> UploadFile(AudioFileContract data, StreamInfo file)
        {
            var context = OperationContext.Current;
            return await StorageLogic.UploadFile(context, file, data);
        }
    }
}
