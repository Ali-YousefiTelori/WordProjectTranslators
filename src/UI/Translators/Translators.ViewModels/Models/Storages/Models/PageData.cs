using System;
using System.Collections.Generic;
using System.Text;

namespace Translators.Models.Storages.Models
{
    public class PageValue
    {
        public long Id { get; set; }
        public long ParentId { get; set; }
        public PageType PageType { get; set; } = PageType.None;

        public string GetKey()
        {
            return $"{PageType}"; 
        }
    }

    public class PageData
    {
        public List<PageValue> Pages { get; set; } = new List<PageValue>();
        public PageType CurrentPage { get; set; } = PageType.None;
    }
}
