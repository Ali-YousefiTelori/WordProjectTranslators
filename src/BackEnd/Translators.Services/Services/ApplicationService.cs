using SignalGo.Server.Models;
using SignalGo.Shared.DataTypes;
using SignalGo.Shared.Http;
using Translators.Contracts.Common;
using Translators.Contracts.Common.DataTypes;
using Translators.Contracts.Responses;
using Translators.Database.Contexts;
using Translators.Database.Entities;
using Translators.Logics;

namespace Translators.Services
{
    [ServiceContract("Application", ServiceType.HttpService, InstanceType.SingleInstance)]
    [ServiceContract("Application", ServiceType.ServerService, InstanceType.SingleInstance)]
    public class ApplicationService
    {
        public async Task<MessageContract<AppVersionResponseContract>> GetAppVersion(ApplicationType applicationType)
        {
            if (applicationType == ApplicationType.None)
                applicationType = ApplicationType.Android;
            return await new LogicBase<TranslatorContext, AppVersionResponseContract, AppVersionEntity>().FirstOrDefault(q => q.Where(x => x.ApplicationType == applicationType));
        }

        public ActionResult DownloadLastVersion(string fileName)
        {
            var context = OperationContext.Current;
            try
            {
                string path = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName, "Application", fileName);

                if (!File.Exists(path))
                {
                    context.HttpClient.Status = System.Net.HttpStatusCode.NotFound;
                    return $"File {fileName} Not found";
                }
                var fileInfo = new FileInfo(path);
                context.HttpClient.ResponseHeaders.Add("Content-Disposition", $"attachment; filename=\"{fileName}\"");
                context.HttpClient.ResponseHeaders.Add("Content-Length", fileInfo.Length.ToString());
                context.HttpClient.ResponseHeaders.Add("Content-Type", MimeTypes.MimeTypeMap.GetMimeType(fileName));
                return new FileActionResult(fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            }
            catch (Exception ex)
            {
                context.HttpClient.Status = System.Net.HttpStatusCode.InternalServerError;
                return new ActionResult(ex.ToString());
            }
        }
    }
}
