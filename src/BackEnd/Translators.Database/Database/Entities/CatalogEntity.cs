using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Database.Entities
{
    /// <summary>
    /// catalog of any books
    /// its like surah
    /// </summary>
    public class CatalogEntity
    {
        public long Id { get; set; }

        public int Number { get; set; }
        public int StartPageNumber { get; set; }

        public List<ValueEntity> Names { get; set; }

        public long BookId { get; set; }
        public BookEntity Book { get; set; }

        public List<PageEntity> Pages { get; set; }
        public List<ParagraphEntity> Paragraphs { get; set; }
    }
}
