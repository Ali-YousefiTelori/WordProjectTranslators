using System;
using System.Threading.Tasks;

namespace Translators.Models.Interfaces
{
    public interface IDownloader
    {
        Action<double> Progress { get; set; }
        Task<bool> Download();
        Task<bool> Extract();
        void DeleteDownloadedFile();
    }
}
