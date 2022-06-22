using System;
using System.IO;
using System.Threading.Tasks;
using Translators.Models.Storages.Models;

namespace Translators.Models.Storages
{
    internal class ApplicationBookData : ApplicationStorageBase<LocalStorageData>
    {
        public void Initialize(string fileName)
        {
            IsLoading = false;
            var directory = Path.Combine(GetFolderPath(), "Temp");
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            FilePath = Path.Combine(directory, $"{fileName}.json");
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
