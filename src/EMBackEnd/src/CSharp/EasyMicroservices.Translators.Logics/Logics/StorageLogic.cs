using EasyMicroservices.ServiceContracts;
using EasyMicroservices.TranslatorsMicroservice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Database.Entities;
using UltraStreamGo;

namespace Translators.Logics
{
    public class StorageLogic
    {
        AppUnitOfWork _appUnitOfWork;
        public StorageLogic(AppUnitOfWork appUnitOfWork)
        {
            _appUnitOfWork = appUnitOfWork;
            DirectoryManager = DirectoryManager<long>.Default;
            DirectoryManager.DefaultFolderPath = _appUnitOfWork.GetConfiguration().GetSection("AppSetting").GetValue<string>("StoragePath");
            DirectoryManager.IsCaseSensitive = false;
        }

        public static DirectoryManager<long> DirectoryManager { get; set; }

        public FileStreamResult GetFile(HttpContext context, long fileId, string password)
        {
            try
            {
                // UltraStreamGo File Info
                FileInfo<long> fileInfo = null;
                // if file exist in storage
                if (DirectoryManager.IsExist(fileId, password))
                    fileInfo = DirectoryManager.GetFileInfo(fileId, password);
                else
                {
                    Console.WriteLine($"File not found {fileId} {password}");
                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
                    return new FileStreamResult(new MemoryStream(), "");
                }

                if (context.Request.Headers.TryGetValue("if-none-match", out StringValues values))
                {
                    if (values.FirstOrDefault() == fileInfo.LastUpdateDateTime.Ticks.ToString())
                    {
                        Console.WriteLine($"File NotModified {fileId} {password}");
                        context.Response.StatusCode = (int)System.Net.HttpStatusCode.NotModified;
                        return new FileStreamResult(new MemoryStream(), "");
                    }
                }

                Console.WriteLine($"File downloading {fileId} {password}", ConsoleColor.Green);
                context.Response.Headers.Add("content-disposition", $"attachment; filename=\"" + fileInfo.FileName + "\"");
                context.Response.Headers.Add("Content-Length", fileInfo.FileSize.ToString());
                context.Response.Headers.Add("Content-Type", fileInfo.DataType);
                context.Response.Headers.Add("Cache-Control", "max-age=604800");//604800 sec = 1 week
                context.Response.Headers.Add("Date", fileInfo.LastUpdateDateTime.ToUniversalTime().ToString("R"));
                context.Response.Headers.Add("ETag", $"{fileInfo.LastUpdateDateTime.Ticks}");

                return new FileStreamResult(DirectoryManager.GetFileStream(fileId, 0), fileInfo.DataType);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return new FileStreamResult(new MemoryStream(Encoding.UTF8.GetBytes(ex.ToString())), "");
            }
        }

        public async Task<MessageContract<AudioFileContract>> UploadFile(HttpContext context, string fileName, Stream file, AudioFileContract fileContract)
        {
            try
            {
                using var dbcontext = _appUnitOfWork.GetContext();
                // set file informations
                fileContract.Password = Guid.NewGuid().ToString();

                var addFileResult = await new LogicBase<AudioEntity>(dbcontext).Add(new AudioEntity()
                {
                    FileName = fileName,
                    IsMain = fileContract.IsMain,
                    LanguageId = fileContract.LanguageId,
                    PageId = fileContract.PageId,
                    TranslatorId = fileContract.TranslatorId,
                    Password = fileContract.Password,
                    AudioReaderId = fileContract.AudioReaderId,
                    ParagraphId = fileContract.ParagraphId,
                });
                if (!addFileResult)
                    return addFileResult.ToContract<AudioFileContract>();

                var addedResult = await UploadToExistingFile(context, fileName, file, addFileResult.Result.Id);
                if (!addedResult)
                    return addedResult.ToContract<AudioFileContract>();
                fileContract.Id = addedResult.Result.Id;

                fileContract.Url = $"{_appUnitOfWork.GetConfiguration().GetSection("AppSetting").GetValue<string>("Domain")}/Storage/DownloadFile?fileId={fileContract.Id}&password={fileContract.Password}";
                return fileContract;
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return ex;
            }
        }

        public async Task<MessageContract<AudioEntity>> UploadToExistingFile(HttpContext context, string fileName, Stream file, long id)
        {
            try
            {
                using var dbcontext = _appUnitOfWork.GetContext();
                var addFileResult = await new LogicBase<AudioEntity>(dbcontext).Find(q => q.Where(x => x.Id == id));
                if (!addFileResult)
                    return addFileResult.ToContract<AudioEntity>();

                // UltraStreamGo File Info
                FileInfo<long> fileInfo = new FileInfo<long>()
                {
                    // set file id
                    Id = addFileResult.Result.Id,
                    // set file name
                    FileName = fileName,
                    // set file size
                    FileSize = file.Length,
                    // set file type
                    DataType = MimeTypes.MimeTypeMap.GetMimeType(fileName),

                    // set Created Date Time
                    //CreatedDateTime = DateTime.Now,

                    // set updated time
                    LastUpdateDateTime = DateTime.Now,
                    // set access password for file
                    Password = addFileResult.Result.Password
                };
                // start uploading file to storage server and set result of operation
                StreamIdentifierFileUploadResult result = await StorageLogic.DirectoryManager.StartUpload(fileInfo, file, 0, fileInfo.FileSize, trowException: true);
                // if upload result is not success
                if (result != StreamIdentifierFileUploadResult.Success)
                {
                    return FailedReasonType.StreamError;
                }

                return addFileResult;
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return ex;
            }
        }
    }
}
