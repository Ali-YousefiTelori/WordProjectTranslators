using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Translators.Models.Storages
{
    internal class ApplicationStorage
    {
        static ApplicationStorage()
        {
            FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ApplicationStorage.json");
            Initialize();
        }

        static string FilePath { get; set; }
        public static ApplicationStorage Current { get; set; }
        static ConcurrentDictionary<string, LocalStorageData> LocalStorageItems { get; set; } = new ConcurrentDictionary<string, LocalStorageData>();

        public List<LocalStorageData> LocalStorages { get; set; }

        public static void Initialize()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    Current = JsonConvert.DeserializeObject<ApplicationStorage>(File.ReadAllText(FilePath, System.Text.Encoding.UTF8));
                }
            }
            catch (Exception ex)
            {

            }

            if (Current == null)
                Current = new ApplicationStorage()
                {
                    LocalStorages = new List<LocalStorageData>()
                };
            foreach (var localStorage in Current.LocalStorages)
            {
                LocalStorageItems.TryAdd(localStorage.Key, localStorage);
            }
        }

        static void Save()
        {
            try
            {
                File.WriteAllText(FilePath, JsonConvert.SerializeObject(Current), System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {

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
                var find = Current.LocalStorages.FirstOrDefault(x => x.Key == key);
                if (find != null)
                {
                    Current.LocalStorages.Remove(find);
                }
                Current.LocalStorages.Add(newValue);
                Save();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
