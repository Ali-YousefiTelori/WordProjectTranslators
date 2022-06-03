using Newtonsoft.Json;
using System.Diagnostics;

namespace Translators.Models
{
    public class ConfigData
    {
        public static ConfigData Current { get; set; }
        public string ConnectionString { get; set; }

        public static async Task LoadAsync()
        {
            var savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config.json");
            Debug.WriteLine(savePath);
            Current = JsonConvert.DeserializeObject<ConfigData>(await File.ReadAllTextAsync(savePath));
        }

        public static void Load()
        {
            if (Current == null)
                LoadAsync().GetAwaiter().GetResult();
        }
    }
}
