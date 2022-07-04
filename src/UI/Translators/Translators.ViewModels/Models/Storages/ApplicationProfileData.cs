using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Translators.Models.Storages.Models;

namespace Translators.Models.Storages
{
    public class ApplicationProfileData : ApplicationStorageBase<ApplicationProfile>
    {
        public ApplicationProfileData()
        {
            FilePath = Path.Combine(GetFolderPath(), "ApplicationProfileData.json");
        }

        public static ApplicationProfileData Current { get; set; } = new ApplicationProfileData();

        public void Save()
        {
            _ = SaveFile();
        }
    }
}
