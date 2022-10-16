using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Translators.Models.Storages
{
    public class ApplicationStorageBase<T>
        where T : new()
    {
        public ApplicationStorageBase()
        {
            IsLoading = true;
        }

        public T Value { get; set; } = new T();

        protected string FilePath { get; set; }

        protected bool IsLoading = false;
        bool DoSaveAfterLoad = false;
        protected string GetFolderPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
        }

        bool IsInitialized = false;
        public async Task BaseInitialize()
        {
            if (IsInitialized)
                return;
            IsInitialized = true;
            try
            {
                await LoadFile(FilePath);
            }
            finally
            {
                IsLoading = false;
            }
            if (DoSaveAfterLoad)
                await SaveFile();
        }

        protected async Task LoadFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    await Load(JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath, System.Text.Encoding.UTF8)));
                }
            }
            catch (Exception ex)
            {

            }
        }

        public virtual async Task Load(T value)
        {
            Value = value;
        }

        SemaphoreSlim SemaphoreSlim { get; set; } = new SemaphoreSlim(1);
        bool isSaving = false;

        protected async Task SaveFile()
        {
            try
            {
                if (IsLoading)
                {
                    DoSaveAfterLoad = true;
                    return;
                }
                if (isSaving)
                    return;
                isSaving = true;
                await SemaphoreSlim.WaitAsync();
                await Task.Delay(1000);
                isSaving = false;
                File.WriteAllText(FilePath, JsonConvert.SerializeObject(Value), System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }

        static HttpClient HttpClient { get; set; } = new HttpClient();
        public async Task<string> DownloadFile(string uri)
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    if (new FileInfo(FilePath).Length > 0)
                        return FilePath;
                }
                using var contentResponse = await HttpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
                using var stream = await contentResponse.Content.ReadAsStreamAsync();

                long contentLength = 0;
                if (contentResponse.Content.Headers.TryGetValues("content-length", out IEnumerable<string> values) && long.TryParse(values?.FirstOrDefault(), out long length))
                    contentLength = length;
                if (contentLength > 0)
                {
                    using var fileStream = File.Create(FilePath);
                    await ReadAllBytesAsync(fileStream, stream, contentLength);
                }
                else
                    File.WriteAllBytes(FilePath, await contentResponse.Content.ReadAsByteArrayAsync());
            }
            catch
            {
                if (File.Exists(FilePath))
                    File.Delete(FilePath);
                throw;
            }
            finally
            {
            }
            return FilePath;
        }

        static async Task ReadAllBytesAsync(Stream fileStream, Stream networkStream, long length)
        {
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
                await fileStream.WriteAsync(readBytes, 0, readCount);
                writed += readCount;
            }
        }

        public async Task<Stream> DownloadFileStream(string uri)
        {
            var stream = new MemoryStream(File.ReadAllBytes(await DownloadFile(uri)));
            return stream;
        }

        public string SaveFile(byte[] data)
        {
            try
            {
                File.WriteAllBytes(FilePath, data);
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return FilePath;
        }
    }
}
