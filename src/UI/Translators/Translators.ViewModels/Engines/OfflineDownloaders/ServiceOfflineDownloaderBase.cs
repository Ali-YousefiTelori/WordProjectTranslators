using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Models.Interfaces;

namespace Translators.Engines.OfflineDownloaders
{
    public abstract class ServiceOfflineDownloaderBase : IDownloader
    {
        static HttpClient HttpClient { get; set; } = new HttpClient();

        public string ServiceAddress { get; set; }
        public string SaveToFileAddress { get; set; }
        public Action<double> Progress { get; set; }

        protected string GetFolderPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
        }

        public virtual async Task<bool> Download()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ServiceAddress);
                using var response = await request.GetResponseAsync();
                using var stream = response.GetResponseStream();
                var fileInfo = new FileInfo(SaveToFileAddress);
                //use cache downloaded
                if (fileInfo.Exists && fileInfo.Length == response.ContentLength)
                {
                    CalculateProgress(fileInfo.Length, fileInfo.Length);
                    return true;
                }
                using (var fileStream = fileInfo.OpenWrite())
                {
                    fileStream.Seek(0, SeekOrigin.Begin);
                    var length = response.ContentLength;
                    long downloadedSize = 0;
                    while (downloadedSize < length)
                    {
                        byte[] buffer = new byte[1024 * 500];
                        var readCount = await stream.ReadAsync(buffer, 0, buffer.Length);
                        await fileStream.WriteAsync(buffer, 0, readCount);
                        downloadedSize += readCount;
                        CalculateProgress(downloadedSize, length);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected void CalculateProgress(double downloadedSize, double length)
        {
            Progress?.Invoke(downloadedSize / length);
        }

        double Length { get; set; }
        double ProgressLength { get; set; }
        protected void CalculateLength(double length)
        {
            Length = length;
            ProgressLength = 0;
        }

        protected void AddProgress()
        {
            ProgressLength++;
            CalculateProgress(ProgressLength, Length);
        }

        protected T DeserializeFromFile<T>()
        {
            using (var compressedStream = new FileStream(SaveToFileAddress, FileMode.Open))
            {
                using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                {
                    using (StreamReader reader = new StreamReader(zipStream))
                    using (JsonTextReader jsonReader = new JsonTextReader(reader))
                    {
                        JsonSerializer ser = new JsonSerializer();
                        var result = ser.Deserialize<T>(jsonReader);
                        return result;
                    }
                }
            }
        }

        protected static MessageContract<T> ToMessageContract<T>(T result)
        {
            MessageContract<T> messageContract = new MessageContract<T>();
            messageContract.Result = result;
            messageContract.IsSuccess = true;
            return messageContract;
        }

        public abstract Task<bool> Extract();
        public void DeleteDownloadedFile()
        {
            if (File.Exists(SaveToFileAddress))
            {
                try
                {
                    File.Delete(SaveToFileAddress);
                }
                catch
                {

                }
            }
        }
    }
}
