using MimeKit;
using SignalGo.Server.Models;
using SignalGo.Shared.Http;
using SignalGo.Shared.Models;
using Translators.Contracts.Common;
using Translators.Database.Entities;
using Translators.Models;
using UltraStreamGo;

namespace Translators.Logics
{
    public class StorageLogic
    {
        static StorageLogic()
        {
            DirectoryManager = DirectoryManager<long>.Default;
            DirectoryManager.DefaultFolderPath = ConfigData.Current.StoragePath;
            DirectoryManager.IsCaseSensitive = false;
        }

        public static DirectoryManager<long> DirectoryManager { get; set; }

        public static ActionResult GetFile(OperationContext context, long fileId, string password)
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
                    context.HttpClient.Status = System.Net.HttpStatusCode.NotFound;
                    return $"File Not found";
                }

                if (context.HttpClient.RequestHeaders.TryGetValue("if-none-match", out string[] values))
                {
                    if (values.FirstOrDefault() == fileInfo.LastUpdateDateTime.Ticks.ToString())
                    {
                        Console.WriteLine($"File NotModified {fileId} {password}");
                        context.HttpClient.Status = System.Net.HttpStatusCode.NotModified;
                        return new ActionResult("");
                    }
                }

                Console.WriteLine($"File downloading {fileId} {password}", ConsoleColor.Green);
                context.HttpClient.ResponseHeaders.Add("content-disposition", $"attachment; filename=\"" + fileInfo.FileName + "\"");
                context.HttpClient.ResponseHeaders.Add("Content-Length", fileInfo.FileSize.ToString());
                context.HttpClient.ResponseHeaders.Add("Content-Type", fileInfo.DataType);
                context.HttpClient.ResponseHeaders.Add("Cache-Control", "max-age=604800");//604800 sec = 1 week
                context.HttpClient.ResponseHeaders.Add("Date", fileInfo.LastUpdateDateTime.ToUniversalTime().ToString("R"));
                context.HttpClient.ResponseHeaders.Add("ETag", $"{fileInfo.LastUpdateDateTime.Ticks}");

                return new FileActionResult(DirectoryManager.GetFileStream(fileId, 0));
            }
            catch (Exception ex)
            {
                context.HttpClient.Status = System.Net.HttpStatusCode.InternalServerError;
                return new ActionResult(ex.ToString());
            }
        }

        public static async Task<MessageContract<AudioFileContract>> UploadFile(OperationContext context, BaseStreamInfo file, AudioFileContract fileContract)
        {
            try
            {
                // set file informations
                fileContract.Password = Guid.NewGuid().ToString();

                var addFileResult = await new TranslatorLogicBase<AudioEntity>().Add(new AudioEntity()
                {
                    FileName = file.FileName,
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

                var addedResult = await UploadToExistingFile(context, file, addFileResult.Result.Id);
                if (!addedResult)
                    return addedResult.ToContract<AudioFileContract>();
                fileContract.Id = addedResult.Result.Id;

                fileContract.Url = $"{ConfigData.Current.Domain}/Storage/DownloadFile?fileId={fileContract.Id}&password={fileContract.Password}";
                return fileContract;
            }
            catch (Exception ex)
            {
                if (context.Client is HttpClientInfo httpClient)
                    httpClient.Status = System.Net.HttpStatusCode.InternalServerError;
                return ex;
            }
        }

        public static async Task<MessageContract<AudioEntity>> UploadToExistingFile(OperationContext context, BaseStreamInfo file, long id)
        {
            try
            {
                var addFileResult = await new TranslatorLogicBase<AudioEntity>().Find(q => q.Where(x => x.Id == id));
                if (!addFileResult)
                    return addFileResult.ToContract<AudioEntity>();

                // UltraStreamGo File Info
                FileInfo<long> fileInfo = new FileInfo<long>()
                {
                    // set file id
                    Id = addFileResult.Result.Id,
                    // set file name
                    FileName = file.FileName,
                    // set file size
                    FileSize = file.Length.GetValueOrDefault(),
                    // set file type
                    DataType = MimeTypes.GetMimeType(file.FileName),

                    // set Created Date Time
                    //CreatedDateTime = DateTime.Now,

                    // set updated time
                    LastUpdateDateTime = DateTime.Now,
                    // set access password for file
                    Password = addFileResult.Result.Password
                };
                // start uploading file to storage server and set result of operation
                StreamIdentifierFileUploadResult result = await StorageLogic.DirectoryManager.StartUpload(fileInfo, file.Stream, 0, fileInfo.FileSize, trowException: true);
                // if upload result is not success
                if (result != StreamIdentifierFileUploadResult.Success)
                {
                    return FailedReasonType.StreamError;
                }

                return addFileResult;
            }
            catch (Exception ex)
            {
                if (context?.Client is HttpClientInfo httpClient)
                    httpClient.Status = System.Net.HttpStatusCode.InternalServerError;
                return ex;
            }
        }
    }
}
