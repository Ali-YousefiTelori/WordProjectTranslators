using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Translators.ViewModels
{
    public class BaseCollectionViewModel<T> : BaseViewModel
    {
        public ObservableCollection<T> Items { get; set; } = new ObservableCollection<T>();

        public void InitialData(IEnumerable<T> items)
        {
            Items.Clear();
            foreach (var category in items)
            {
                Items.Add(category);
            }
        }
    }
}
