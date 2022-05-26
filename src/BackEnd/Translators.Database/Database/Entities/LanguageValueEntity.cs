using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Database.Entities
{
    public class LanguageValueEntity
    {
        public long Id { get; set; }
        /// <summary>
        /// is main of the value or false its translated
        /// </summary>
        public bool IsMain { get; set; }
        public string Value { get; set; }

        public long LanguageId { get; set; }
        public LanguageEntity Language { get; set; }

        public List<CategoryEntity> Categories { get; set; }
        public List<BookEntity> Books { get; set; }
        public List<CatalogEntity> Catalogs { get; set; }
    }
}
