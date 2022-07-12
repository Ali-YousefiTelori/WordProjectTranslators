using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Translators.Helpers;

namespace Translators.ViewModels
{
    public class BaseCollectionViewModel<T> : BaseViewModel
    {
        protected static string SearchChars = "اآبتثجچحخدذرزسشصضطظعغفقکلمنوهیكڪىيٱءإئۀةؤأ0123456789";
        protected string _SearchText = "";
        public virtual string SearchText
        {
            get => _SearchText;
            set
            {
                _SearchText = value;
                OnPropertyChanged(nameof(SearchText));
                Search();
            }
        }

        string _Title;
        public string Title
        {
            get => _Title;
            set
            {
                _Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public List<T> OfflineItems { get; set; }

        public ObservableCollection<T> Items { get; set; } = new ObservableCollection<T>();

        public void InitialData(IEnumerable<T> items)
        {
            AsyncHelper.RunOnUI(() =>
            {
                Items.Clear();
                foreach (var category in items)
                {
                    Items.Add(category);
                }
            });
        }

        public virtual void Search()
        {

        }

        /// <summary>
        /// not filtered items, all items
        /// </summary>
        public List<T> GetRealItems()
        {
            if (OfflineItems == null || OfflineItems.Count == 0)
                return Items.ToList();
            return OfflineItems;
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
