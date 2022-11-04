using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Models.Interfaces
{
    public interface IApplicationManager
    {
        Task<long> GetBuildNumber();
        Task DownloadNewVersion();
        Task KeepScreenOn(bool isKeepScreenOn);
    }
}
