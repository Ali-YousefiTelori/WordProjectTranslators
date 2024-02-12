using EasyMicroservices.TranslatorsMicroservice;
using Microsoft.AspNetCore.Mvc;

namespace Translators.Services
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StorageController : ControllerBase
    {
        AppUnitOfWork _appUnitOfWork;
        public StorageController(AppUnitOfWork appUnitOfWork)
        {
            _appUnitOfWork = appUnitOfWork;
        }

        [HttpGet]
        public Task<FileStreamResult> DownloadFile(long fileId, string password)
        {
            return Task.FromResult(_appUnitOfWork.GetStorageLogic().GetFile(HttpContext, fileId, password));
        }

        //[AuthenticationSecurityPermission(PermissionType.Admin)]
        //public async Task<MessageContract<AudioFileContract>> UploadFile(AudioFileContract data, Stream file)
        //{
        //    return await _appUnitOfWork.GetStorageLogic().UploadFile(HttpContext,, data, file, data);
        //}
    }
}
