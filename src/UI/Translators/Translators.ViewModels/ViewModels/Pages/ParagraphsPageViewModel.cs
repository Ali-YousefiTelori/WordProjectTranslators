using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranslatorApp.GeneratedServices;
using Translators.Helpers;
using Translators.Models.Interfaces;

namespace Translators.ViewModels.Pages
{
    public class ParagraphsPageViewModel : ParagraphBaseViewModel<SearchResultModel>
    {
        public ParagraphsPageViewModel()
        {
            TouchedCommand = CommandHelper.Create<SearchResultModel>(Touch);
        }

        public ICommand<SearchResultModel> TouchedCommand { get; set; }

        public void Initialize(List<SearchValueContract> values)
        {
            bool isEven = false;
            InitialData(values.Select(v =>
            {
                var value = Map(v);
                value.IsEven = isEven;
                isEven = !isEven;
                return value;
            }));
        }

        private async Task Touch(SearchResultModel paragraph)
        {
            if (!IsOnSelectionMode(paragraph))
                await TouchBase(paragraph, true, false);
        }
    }
}