using System.Collections.Generic;

namespace Translators.Models.Storages.Models
{
    public class PageValue
    {
        public long Id { get; set; }
        public long DataId { get; set; }
        public long ParentId { get; set; }
        public PageType PageType { get; set; } = PageType.None;

        public string GetKey()
        {
            return $"{PageType}";
        }
    }

    public class PageData : BaseModel
    {
        string _Name;
        public string Name
        {
            get => _Name; set
            {
                _Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _title;
        public string Title
        {
            get => _title; set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public bool IsPinned { get; set; }
        public List<PageValue> Pages { get; set; } = new List<PageValue>();
        public PageType CurrentPage { get; set; } = PageType.None;
    }

    public class ReadingData
    {
        public List<PageData> Items { get; set; } = new List<PageData>();
    }
}
