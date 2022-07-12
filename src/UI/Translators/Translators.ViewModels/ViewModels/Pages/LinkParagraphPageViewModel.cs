using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Helpers;
using Translators.Models;
using Translators.Models.Interfaces;
using Translators.ServiceManagers;

namespace Translators.ViewModels.Pages
{
    public class LinkGroup : BaseModel
    {
        public LinkGroupContract Contract { get; set; }

        bool _IsSelected;
        public bool IsSelected
        {
            get => _IsSelected;
            set
            {
                _IsSelected = value;
                SelectedDateTime = DateTime.Now;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
        public DateTime SelectedDateTime { get; set; }
    }

    public class LinkParagraphPageViewModel : BaseCollectionViewModel<LinkGroup>
    {
        public LinkParagraphPageViewModel()
        {
            SaveCommand = CommandHelper.Create(Save);
            SelectedCommand = CommandHelper.Create<LinkGroup>(SelectedChanged);
            _ = LoadData();
        }

        public ICommand SaveCommand { get; set; }
        public ICommand<LinkGroup> SelectedCommand { get; set; }

        string _ContentToLink;
        public string ContentToLink
        {
            get => _ContentToLink;
            set
            {
                _ContentToLink = value;
                OnPropertyChanged(nameof(ContentToLink));
            }
        }

        ParagraphBaseModel LinkToParagraphBaseModel { get; set; }
        public void Initialize(ParagraphBaseModel paragraphBaseModel)
        {
            LinkToParagraphBaseModel = paragraphBaseModel;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("آیا می‌خواهید");
            builder.AppendLine();
            builder.AppendLine(TranslatorService.ParagraphForLink.TranslatedValue);
            if (!string.IsNullOrEmpty(TranslatorService.ParagraphForLink.DisplayName))
                builder.AppendLine(TranslatorService.ParagraphForLink.DisplayName);
            builder.AppendLine();
            builder.AppendLine("را به");
            builder.AppendLine();
            builder.AppendLine(paragraphBaseModel.TranslatedValue);
            if (!string.IsNullOrEmpty(paragraphBaseModel.DisplayName))
                builder.AppendLine(paragraphBaseModel.DisplayName);
            builder.AppendLine();
            builder.AppendLine("لینک کنید؟");
            ContentToLink = builder.ToString();
        }

        public override void Search()
        {
            var searchText = FixArabicForSearch(SearchText);
            Filter(x => FixArabicForSearch(x.Contract.Title).Contains(searchText), x =>
            {
                return 0;
            });
        }

        public override async Task FetchData(bool isForce = false)
        {
            var groups = await TranslatorService.GetParagraphService(true).GetLinkGroupsAsync();
            if (groups.IsSuccess)
            {
                InitialData(groups.Result.Select(x => new LinkGroup()
                {
                    Contract = x
                }));
            }
            else
                await AlertContract(groups);
        }

        private async Task Save()
        {
            if (await AlertHelper.DisplayQuestion("لینک", ContentToLink))
            {
                var find = GetRealItems().FirstOrDefault(x => x.IsSelected);
                var result = await TranslatorService.GetParagraphService(true).LinkParagraphAsync(new Contracts.Requests.LinkParagraphRequestContract()
                {
                    Title = find == null ? SearchText : find.Contract.Title,
                    FromParagraphId = TranslatorService.ParagraphForLink.Id,
                    ToParagraphId = LinkToParagraphBaseModel.Id
                });
                if (result.IsSuccess)
                {
                    await AlertHelper.Alert("لینک", "آیه‌ی مورد نظر با موفقیت لینک شد.");
                }
                else
                {
                    await AlertHelper.Alert("خطا در لینک", result.Error.Message);
                }
            }
        }

        private Task SelectedChanged(LinkGroup link)
        {
            if (link.IsSelected && DateTime.Now - link.SelectedDateTime > TimeSpan.FromMilliseconds(500))
                link.IsSelected = false;
            foreach (var item in GetRealItems().Where(x => x != link))
            {
                item.IsSelected = false;
            }
            return Task.CompletedTask;
        }
    }
}
