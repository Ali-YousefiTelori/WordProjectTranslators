using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Contracts.Requests
{
    public class AdvancedSearchFilterRequestContract
    {
        public string Search { get; set; }
        /// <summary>
        /// search in books
        /// </summary>
        public List<long> BookIds { get; set; }

        public bool SkipSearchInTranslates { get; set; }
        public bool SkipSearchInMain { get; set; }
        /// <summary>
        /// جستجو به شکل کل جمله
        /// </summary>
        public bool DoFullWordsSearch { get; set; }
    }
}
