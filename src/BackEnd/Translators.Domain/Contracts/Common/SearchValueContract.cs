using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Contracts.Common
{
    public class SearchValueContract
    {
        public bool HasLink { get; set; }
        public long Number { get; set; }
        public ValueContract Value { get; set; }
        public long ParagraphId { get; set; }
        public List<WordContract> ParagraphWords { get; set; }
        public long PageId { get; set; }
        public long PageNumber { get; set; }
        public long CatalogId { get; set; }
        public List<ValueContract> CatalogNames { get; set; }
        public long BookId { get; set; }
        public List<ValueContract> BookNames { get; set; }
    }
}
