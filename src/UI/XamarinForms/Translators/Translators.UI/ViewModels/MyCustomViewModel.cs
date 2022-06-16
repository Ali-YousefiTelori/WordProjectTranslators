using Translators.ViewModels.Pages;
using Xamarin.Forms;

namespace Translators.UI.ViewModels
{
    public class MyCustomViewModel : CategoryViewModel
    {
        public MyCustomViewModel()
        {

            ItemTapped = new Command<object>(OnItemSelected);
        }

        private void OnItemSelected(object obj)
        {

        }

        public Command<object> ItemTapped { get; }

    }
}
