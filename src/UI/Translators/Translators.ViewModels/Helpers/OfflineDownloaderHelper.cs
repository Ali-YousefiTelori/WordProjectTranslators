using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Translators.Engines.OfflineDownloaders;
using Translators.Models.Interfaces;

namespace Translators.Helpers
{
    public class OfflineDownloaderHelper
    {
        static OfflineDownloaderHelper()
        {
            Downloaders.Add(new BookServiceOfflineDownloader());
            Downloaders.Add(new ChapterServiceOfflineDownloader());
            Downloaders.Add(new PageServiceOfflineDownloader());
            //Downloaders.Add(new ParagraphServiceOfflineDownloader());
        }

        public static OfflineDownloaderHelper Current { get; set; } = new OfflineDownloaderHelper();
        static List<IDownloader> Downloaders { get; set; } = new List<IDownloader>();
        public Action DataChanged { get; set; }

        public int CompletedServiceCount { get; set; }
        public int ServiceCount { get; set; }
        public double CompletePercent { get; set; }
        public bool IsExtracting { get; set; }

        bool _isStarted = false;
        public async Task<bool?> Start()
        {
            if (_isStarted)
                return null;
            try
            {
                _isStarted = true;

                if (!await DownloadAll())
                    return false;
                if (!await ExtractAll())
                    return false;
            }
            finally
            {
                _isStarted = false;
            }
            return true;
        }

        async Task<bool> DownloadAll()
        {
            IsExtracting = false;
            CompletedServiceCount = 0;
            ServiceCount = Downloaders.Count;
            DataChanged?.Invoke();
            foreach (var downloader in Downloaders)
            {
                downloader.Progress = (value) =>
                {
                    CompletePercent = value;
                    DataChanged?.Invoke();
                };
                if (!await downloader.Download())
                    return false;
                CompletedServiceCount++;
                DataChanged?.Invoke();
            }
            return true;
        }

        async Task<bool> ExtractAll()
        {
            IsExtracting = true;
            CompletedServiceCount = 0;
            DataChanged?.Invoke();
            foreach (var downloader in Downloaders)
            {
                if (!await downloader.Extract())
                    return false;
                CompletedServiceCount++;
                DataChanged?.Invoke();
            }
            return true;
        }

        public void DeleteCachedDownloadedFiles()
        {
            foreach (var downloader in Downloaders)
            {
                downloader.DeleteDownloadedFile();
            }
        }
    }
}
