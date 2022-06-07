﻿using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Helpers;

namespace Translators.ViewModels.Pages
{
    public class ChapterViewModel : BaseCollectionViewModel<CatalogContract>
    {
        public ChapterViewModel()
        {
            TouchedCommand = new Command<CatalogContract>(Touched);
        }

        long BookId = 0;
        public Command<CatalogContract> TouchedCommand { get; set; }

        public async Task Initialize(long id)
        {
            BookId = id;
            await LoadData();
        }

        public async void Touched(CatalogContract catalog)
        {

        }

        public override async Task FetchData()
        {
            var chapters = await TranslatorService.ChapterServiceHttp.FilterChaptersAsync(BookId);
            if (chapters.IsSuccess)
            {
                InitialData(chapters.Result);
            }
        }
    }
}
