using System.Collections.Generic;
using Translators.Database.Entities.UserPersonalization;
using Translators.Schemas;

namespace Translators.Database.Entities
{
    /// <summary>
    /// page of book
    /// </summary>
    public class PageEntity : PageSchema
    {
        public CatalogEntity Catalog { get; set; }
        public List<ParagraphEntity> Paragraphs { get; set; }
        public List<AudioEntity> Audios { get; set; }
        public List<ReadingEntity> Readings { get; set; }
    }
}
