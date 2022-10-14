using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Database.Entities
{
    /// <summary>
    /// paragraph of a book page
    /// </summary>
    public class ParagraphEntity
    {
        public long Id { get; set; }
        public long Number { get; set; }
        public string AnotherValue { get; set; }

        public long PageId { get; set; }
        public PageEntity Page { get; set; }

        public long CatalogId { get; set; }
        public CatalogEntity Catalog { get; set; }

        public List<WordEntity> Words { get; set; }
        public List<LinkParagraphEntity> LinkParagraphs { get; set; }
    }
}
