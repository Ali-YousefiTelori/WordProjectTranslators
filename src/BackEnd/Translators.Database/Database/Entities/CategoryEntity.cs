using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Database.Entities
{
    /// <summary>
    /// category of books
    /// </summary>
    public class CategoryEntity
    {
        public long Id { get; set; }

        public long NameId { get; set; }
        public LanguageValueEntity Name { get; set; }

        public List<BookEntity> Books { get; set; }
    }
}
