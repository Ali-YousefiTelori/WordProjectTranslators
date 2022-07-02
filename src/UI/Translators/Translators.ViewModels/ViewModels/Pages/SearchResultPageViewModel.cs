using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Converters;
using Translators.Helpers;
using Translators.Models.DataTypes;
using Translators.Models.Interfaces;

namespace Translators.ViewModels.Pages
{
    public class SearchResultModel
    {
        public string MainValue { get; set; }
        public string TranslatedValue { get; set; }
        public bool IsEven { get; set; }
    }

    public class SearchResultPageViewModel : BaseCollectionViewModel<SearchResultModel>
    {
        public SearchResultPageViewModel()
        {
            TouchedCommand = CommandHelper.Create<SearchResultModel>(Touch);
        }

        public ICommand<SearchResultModel> TouchedCommand { get; set; }

        public void Initialize(List<SearchValueContract> values)
        {

            bool isEven = false;
            InitialData(values.Select(v =>
            {
                var displayName = " " + $"({LanguageValueBaseConverter.GetValue(v.BookNames, false, "fa-ir")} - {LanguageValueBaseConverter.GetValue(v.CatalogNames, false, "fa-ir")} - آیه‌ی {v.Number})";
                var value = new SearchResultModel()
                {
                    TranslatedValue = string.Join(" ", v.ParagraphWords.OrderBy(x => x.Index).SelectMany(x => x.Values).Where(x => !x.IsMain && x.Language.Code == "fa-ir").Select(x => x.Value)) + displayName,
                    MainValue = string.Join(" ", v.ParagraphWords.OrderBy(x => x.Index).SelectMany(x => x.Values).Where(x => x.IsMain).Select(x => x.Value)) + displayName,
                    IsEven = isEven
                };
                isEven = !isEven;
                return value;
            }));
        }

        private async Task Touch(SearchResultModel paragraph)
        {
            string displayName = null;

            var selectedType = await AlertHelper.Display<VerseRightClickType>("عملیات", "انصراف", "کپی همه", "کپی آیه", "کپی ترجمه", "رفتن به صفحه...");
            switch (selectedType)
            {
                case VerseRightClickType.CopyAll:
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.AppendLine(paragraph.MainValue);
                        stringBuilder.Append(paragraph.TranslatedValue);
                        stringBuilder.Append(displayName);
                        await ClipboardHelper.CopyText(stringBuilder.ToString());
                        break;
                    }
                case VerseRightClickType.CopyVerse:
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append(paragraph.MainValue);
                        stringBuilder.Append(displayName);
                        await ClipboardHelper.CopyText(stringBuilder.ToString());
                        break;
                    }
                case VerseRightClickType.CopyTranslate:
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append(paragraph.TranslatedValue);
                        stringBuilder.Append(displayName);
                        await ClipboardHelper.CopyText(stringBuilder.ToString());
                        break;
                    }
                case VerseRightClickType.GoToPage:
                    {
                        await AlertHelper.Alert("قابلیت", "این قابلیت هنوز اضافه نشده!");
                        //PageHelper.PushPage
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
