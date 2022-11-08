using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translators.Database.Entities.UserPersonalization;

namespace Translators.Database.Entities
{
    /// <summary>
    /// category of books
    /// </summary>
    public class CategoryEntity
    {
        public long Id { get; set; }

        public List<ValueEntity> Names { get; set; }

        public List<BookEntity> Books { get; set; }
        public List<ReadingEntity> Readings { get; set; }
    }
}
