using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Translators.ViewModels
{
    public class BaseCollectionViewModel<T> : BaseViewModel
    {
        protected static string SearchChars = "اآبتثجچحخدذرزسشصضطظعغفقکلمنوهیكڪىيٱءإئۀةؤأ0123456789";
        string _SearchText = "";
        public string SearchText
        {
            get => _SearchText;
            set
            {
                _SearchText = value;
                OnPropertyChanged(nameof(SearchText));
                Search();
            }
        }

        public List<T> OfflineItems { get; set; }

        public ObservableCollection<T> Items { get; set; } = new ObservableCollection<T>();

        public void InitialData(IEnumerable<T> items)
        {
            Items.Clear();
            foreach (var category in items)
            {
                Items.Add(category);
            }
        }

        public virtual void Search()
        {

        }

        public void Filter(Func<T, bool> canAdd, Func<T, int> getIndex)
        {
            if (OfflineItems == null || OfflineItems.Count == 0)
                OfflineItems = Items.ToList();
            Items.Clear();

            foreach (var offlineItem in OfflineItems.OrderBy(x => getIndex(x)))
            {
                if (canAdd(offlineItem))
                    Items.Add(offlineItem);
            }
        }

        public string CleanText(string text)
        {
            return new string(FixArabicForSearch(text).Where(x => SearchChars.Contains(x)).ToArray());
        }
    }
}
