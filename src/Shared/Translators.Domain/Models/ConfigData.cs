using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Translators.Models
{
    public class ConfigData
    {
        public static ConfigData Current { get; set; }
        public string ConnectionString { get; set; }
        public string Domain { get; set; }
        public string StoragePath { get; set; }
        public static async Task LoadAsync()
        {
            var savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config.json");
            Debug.WriteLine(savePath);
#if (NET6_0)
            Current = JsonConvert.DeserializeObject<ConfigData>(await File.ReadAllTextAsync(savePath));
#else
            Current = JsonConvert.DeserializeObject<ConfigData>(File.ReadAllText(savePath));
#endif
        }

        public static void Load()
        {
            if (Current == null)
                LoadAsync().GetAwaiter().GetResult();
        }
    }
}
