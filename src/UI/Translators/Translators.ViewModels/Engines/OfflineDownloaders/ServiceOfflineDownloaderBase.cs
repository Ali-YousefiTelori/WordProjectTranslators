using Newtonsoft.Json;
using System.IO.Compression;
using Translators.Contracts.Common;
using Translators.Models.Interfaces;
using Translators.Models.Storages;

namespace Translators.Engines.OfflineDownloaders
{
    public abstract class ServiceOfflineDownloaderBase : StreamDownloaderBase, IDownloader
    {
        public string ServiceAddress { get; set; }
        public string SaveToFileAddress { get; set; }
        public Action<double> Progress { get; set; }

        public virtual async Task<bool> Download()
        {
            try
            {
                await DownloadFile(ServiceAddress, SaveToFileAddress, false);
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected override void CalculateProgress(double downloadedSize, double length)
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
