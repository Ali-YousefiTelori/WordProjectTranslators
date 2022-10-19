using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Translators.Models.Storages
{
    public abstract class StreamDownloaderBase
    {
        static HttpClient HttpClient { get; set; } = new HttpClient();
        public async Task<string> DownloadFile(string uri, string filePath)
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    if (File.Exists(filePath))
                    {
                        if (new FileInfo(filePath).Length > 0)
                            return filePath;
                    }
                    using var contentResponse = await HttpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
                    using var stream = await contentResponse.Content.ReadAsStreamAsync();

                    long contentLength = 0;
                    if (contentResponse.Content.Headers.TryGetValues("content-length", out IEnumerable<string> values) && long.TryParse(values?.FirstOrDefault(), out long length))
                        contentLength = length;
                    if (contentLength > 0)
                    {
                        var bytes = await ReadAllBytesAsync(stream, contentLength);
                        File.WriteAllBytes(filePath, bytes);
                    }
                    else
                        File.WriteAllBytes(filePath, await contentResponse.Content.ReadAsByteArrayAsync());
                    break;
                }
                catch
                {
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                    if (i == 2)
                        throw;
                }
                finally
                {
                }
            }
            return filePath;
        }

        public async Task<byte[]> ReadAllBytesAsync(Stream networkStream, long length)
        {
            using var memoryStream = new MemoryStream();
            byte[] readBytes = new byte[1024 * 512];
            long writed = 0;
            while (writed < length)
            {
                int readCount;
                if (readBytes.Length > length - writed)
                    readBytes = new byte[length - writed];
                readCount = await networkStream.ReadAsync(readBytes, 0, readBytes.Length);
                if (readCount <= 0)
                    throw new Exception("Client disconnected!");
                await memoryStream.WriteAsync(readBytes, 0, readCount);
                writed += readCount;
                CalculateProgress(writed, length);
            }
            return memoryStream.ToArray();
        }

        protected virtual void CalculateProgress(double downloadedSize, double length)
        {

        }
    }
}
