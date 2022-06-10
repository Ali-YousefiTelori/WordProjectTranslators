using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Contracts.Common
{
    /// <summary>
    /// page of book
    /// </summary>
    public class PageContract
    {
        public long Id { get; set; }
        public long Number { get; set; }

        public List<ValueContract> CatalogNames { get; set; }
        public long CatalogId { get; set; }

        public List<ParagraphContract> Paragraphs { get; set; }
    }
}
