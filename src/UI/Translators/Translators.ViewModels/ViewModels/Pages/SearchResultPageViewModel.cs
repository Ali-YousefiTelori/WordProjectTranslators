using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Converters;
using Translators.Helpers;
using Translators.Models;
using Translators.Models.Interfaces;

namespace Translators.ViewModels.Pages
{
    public class SearchResultModel : ParagraphBaseModel
    {
        public bool IsEven { get; set; }
    }

    public class SearchResultPageViewModel : ParagraphBaseViewModel<SearchResultModel>
    {
        public SearchResultPageViewModel()
        {
            IsInSearchTab = true;
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
            await TouchBase(paragraph, true);
        }
    }
}
