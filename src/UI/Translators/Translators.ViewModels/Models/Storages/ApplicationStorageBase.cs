using Newtonsoft.Json;

namespace Translators.Models.Storages
{
    public class ApplicationStorageBase<T> : StreamDownloaderBase
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

        public async Task<Stream> DownloadFileStream(string uri, bool doReDownload)
        {
            var stream = new MemoryStream(File.ReadAllBytes(await DownloadFile(uri, FilePath, doReDownload)));
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
