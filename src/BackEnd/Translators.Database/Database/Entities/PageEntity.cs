using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Database.Entities
{
    /// <summary>
    /// page of book
    /// </summary>
    public class PageEntity
    {
        public long Id { get; set; }
        public long Number { get; set; }

        public long CatalogId { get; set; }
        public CatalogEntity Catalog { get; set; }

        public List<ParagraphEntity> Paragraphs { get; set; }
    }
}
