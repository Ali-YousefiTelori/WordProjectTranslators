using System.Collections.Generic;
using Translators.Database.Entities.UserPersonalization;
using Translators.Schemas.Bases;

namespace Translators.Database.Entities
{
    /// <summary>
    /// real books
    /// </summary>
    public class BookEntity : BookSchemaBase
    {
        public List<ValueEntity> Names { get; set; }
        public CategoryEntity Category { get; set; }
        public List<CatalogEntity> Catalogs { get; set; }
        public List<ReadingEntity> Readings { get; set; }
    }
}
