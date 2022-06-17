using Newtonsoft.Json;
using System;
using System.IO;
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

        bool IsLoading = false;
        bool DoSaveAfterLoad = false;
        protected static string GetFolderPath()
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
        protected async Task SaveFile()
        {
            try
            {
                if (IsLoading)
                {
                    DoSaveAfterLoad = true;
                    return;
                }
                await SemaphoreSlim.WaitAsync();
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
    }
}
