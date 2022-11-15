using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Contracts.Common.Authentications;
using Translators.Contracts.Common.DataTypes;
using Translators.Models.Storages.Models;
using Translators.ServiceManagers;

namespace Translators.Models.Storages
{
    public class ApplicationProfileData : ApplicationStorageBase<ApplicationProfile>
    {
        public ApplicationProfileData()
        {
            FilePath = Path.Combine(GetFolderPath(), "ApplicationProfileData.json");
        }

        public static ApplicationProfileData Current { get; set; } = new ApplicationProfileData();

        public override async Task Load(ApplicationProfile value)
        {
            await base.Load(value);
            await Login(Value.Session);
        }

        public void Save()
        {
            _ = SaveFile();
        }

        public static async Task<MessageContract<UserContract>> Login(Guid? session)
        {
            if (session.HasValue)
            {
                var result = await TranslatorService.GetOldAuthenticationService(true).LoginAsync(session.Value);
                if (result.IsSuccess)
                {
                    Current.Value.Session = Guid.Parse(result.Result.Key);
                    TranslatorService.IsAdmin = result.Result.Permissions.Any(x => x == PermissionType.Admin);
                    _ = ApplicationReadingData.SyncReading();
                }
                return result;
            }
            return new MessageContract<UserContract>()
            {
                IsSuccess = false,
                Error = new ErrorContract()
                {
                    Message = "داده ها کامل نیست"
                }
            };
        }

    }
}
