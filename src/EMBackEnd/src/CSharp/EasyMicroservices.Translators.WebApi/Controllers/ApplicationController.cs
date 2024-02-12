using EasyMicroservices.ServiceContracts;
using EasyMicroservices.TranslatorsMicroservice;
using Microsoft.AspNetCore.Mvc;
using Translators.Contracts.Common.DataTypes;
using Translators.Contracts.Responses;
using Translators.Database.Entities;
using Translators.Logics;

namespace Translators.Services
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApplicationController : ControllerBase
    {
        AppUnitOfWork _appUnitOfWork;
        public ApplicationController(AppUnitOfWork appUnitOfWork)
        {
            _appUnitOfWork = appUnitOfWork;
        }

        [HttpPost]
        public async Task<MessageContract<AppVersionResponseContract>> GetAppVersion(ApplicationType applicationType)
        {
            if (applicationType == ApplicationType.None)
                applicationType = ApplicationType.Android;
            using var context = _appUnitOfWork.GetContext();
            return await new LogicBase<AppVersionResponseContract, AppVersionEntity>(context)
                .FirstOrDefault(q => q.Where(x => x.ApplicationType == applicationType));
        }

        [HttpPost]
        public FileStreamResult DownloadLastVersion(string fileName)
        {
            var context = HttpContext;
            string path = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName, "Application", fileName);

            if (!System.IO.File.Exists(path))
            {
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
                throw new Exception("File not found!");
            }
            var fileInfo = new FileInfo(path);
            context.Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{fileName}\"");
            context.Response.Headers.Add("Content-Length", fileInfo.Length.ToString());
            context.Response.Headers.Add("Content-Type", MimeTypes.MimeTypeMap.GetMimeType(fileName));
            return File(fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite), MimeTypes.MimeTypeMap.GetMimeType(fileName));
        }
    }
}
