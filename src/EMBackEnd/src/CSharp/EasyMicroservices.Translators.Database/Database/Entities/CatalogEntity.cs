using System.Collections.Generic;
using Translators.Database.Entities.UserPersonalization;
using Translators.Schemas.Bases;

namespace Translators.Database.Entities
{
    /// <summary>
    /// catalog of any books
    /// its like surah
    /// </summary>
    public class CatalogEntity : CatalogSchemaBase
    {
        public BookEntity Book { get; set; }
        public List<ValueEntity> Names { get; set; }
        public List<PageEntity> Pages { get; set; }
        public List<ParagraphEntity> Paragraphs { get; set; }
        public List<ReadingEntity> Readings { get; set; }
    }
}
