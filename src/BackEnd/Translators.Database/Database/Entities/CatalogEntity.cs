using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Database.Entities
{
    /// <summary>
    /// catalog of any books
    /// </summary>
    public class CatalogEntity
    {
        public long Id { get; set; }

        public int Number { get; set; }
        public int StartPageNumber { get; set; }
        
        public long NameId { get; set; }
        public LanguageValueEntity Name { get; set; }

        public long BookId { get; set; }
        public BookEntity Book { get; set; }

        public List<PageEntity> Pages { get; set; }
    }
}
