using System;
using System.IO;
using System.Threading.Tasks;
using Translators.Models.Storages.Models;

namespace Translators.Models.Storages
{
    internal class ApplicationBookData : ApplicationStorageBase<LocalStorageData>
    {
        public string FolderName { get; set; } = "Temp";
        public string GetCurrentFolderPath()
        {
            var directory = Path.Combine(GetFolderPath(), FolderName);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            return directory;
        }

        public void Initialize(string fileName, string extentions = ".json")
        {
            IsLoading = false;
            FilePath = Path.Combine(GetCurrentFolderPath(), $"{fileName}{extentions}");
        }

        public async Task InitializeLoad(string fileName)
        {
            Initialize(fileName);
            await BaseInitialize();
        }

        public bool TryGet(out string value)
        {
            if (Value?.JsonValue != null)
            {
                value = Value.JsonValue;
                return true;
            }
            value = null;
            return false;
        }

        public async Task Add(string value)
        {
            try
            {
                Value = new LocalStorageData() { JsonValue = value };
                _ = SaveFile();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
