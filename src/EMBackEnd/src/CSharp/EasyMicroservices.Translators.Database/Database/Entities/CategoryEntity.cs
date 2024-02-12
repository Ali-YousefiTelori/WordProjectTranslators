using System.Collections.Generic;
using Translators.Database.Entities.UserPersonalization;
using Translators.Schemas.Bases;

namespace Translators.Database.Entities
{
    /// <summary>
    /// category of books
    /// </summary>
    public class CategoryEntity : CategorySchemaBase
    {
        public List<ValueEntity> Names { get; set; }
        public List<BookEntity> Books { get; set; }
        public List<ReadingEntity> Readings { get; set; }
    }
}
