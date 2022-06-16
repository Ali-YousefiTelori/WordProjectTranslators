using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Translators.Models.Storages.Models;

namespace Translators.Models.Storages
{
    internal class ApplicationBookData : ApplicationStorageBase<List<LocalStorageData>>
    {
        public ApplicationBookData()
        {
            FilePath = Path.Combine(GetFolderPath(), "ApplicationBookData.json");
        }

        static ApplicationBookData()
        {
            _ = Current.BaseInitialize();
        }

        public static ApplicationBookData Current { get; set; } = new ApplicationBookData();
        static ConcurrentDictionary<string, LocalStorageData> LocalStorageItems { get; set; } = new ConcurrentDictionary<string, LocalStorageData>();

        public override async Task Load(List<LocalStorageData> value)
        {
            foreach (var localStorage in value)
            {
                LocalStorageItems.TryAdd(localStorage.Key, localStorage);
            }
        }

        public static bool TryGet(string key, out string value)
        {
            if (LocalStorageItems.TryGetValue(key, out LocalStorageData localStorage))
            {
                value = localStorage.JsonValue;
                return true;
            }
            value = null;
            return false;
        }

        public static async Task Add(string key, string value)
        {
            try
            {
                var newValue = new LocalStorageData() { Key = key, JsonValue = value };
                LocalStorageItems.AddOrUpdate(key, newValue, (k, data) => newValue);
                var find = Current.Value.FirstOrDefault(x => x.Key == key);
                if (find != null)
                {
                    Current.Value.Remove(find);
                }
                Current.Value.Add(newValue);
                _ = Current.SaveFile();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
