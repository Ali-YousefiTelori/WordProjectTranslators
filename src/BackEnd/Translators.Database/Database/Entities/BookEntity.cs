using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Database.Entities
{
    /// <summary>
    /// real books
    /// </summary>
    public class BookEntity
    {
        public long Id { get; set; }

        public List<ValueEntity> Names { get; set; }

        public long CategoryId { get; set; }
        public CategoryEntity Category { get; set; }

        public List<CatalogEntity> Catalogs { get; set; }
    }
}
