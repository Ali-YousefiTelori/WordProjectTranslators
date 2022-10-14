using System.IO;
using System.Threading.Tasks;
using Translators.Models.Storages.Models;
using Translators.ServiceManagers;
using Translators.ViewModels;

namespace Translators.Models.Storages
{
    public class ApplicationSettingData : ApplicationStorageBase<ApplicationSetting>
    {
        public ApplicationSettingData()
        {
            FilePath = Path.Combine(GetFolderPath(), "ApplicationSettingData.json");
        }

        public static ApplicationSettingData Current { get; set; } = new ApplicationSettingData();

        public override async Task Load(ApplicationSetting value)
        {
            BaseViewModel._FontSize = value.FontSize;
            BaseViewModel._PlaybackSpeedRato = value.PlaybackSpeedRato;
            TranslatorService.IsDuplexProtocol = BaseViewModel._UseDuplexProtocol = value.UseDuplexProtocol;
        }

        public void Save()
        {
            Value.FontSize = BaseViewModel._FontSize;
            Value.UseDuplexProtocol = BaseViewModel._UseDuplexProtocol;
            Value.PlaybackSpeedRato = BaseViewModel._PlaybackSpeedRato;
            _ = SaveFile();
        }
    }
}
