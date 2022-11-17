using Translators.Schemas;

namespace Translators.Database.Entities
{
    /// <summary>
    /// paragraph of a book page
    /// </summary>
    public class ParagraphEntity : ParagraphSchema
    {
        public PageEntity Page { get; set; }
        public CatalogEntity Catalog { get; set; }
        public List<WordEntity> Words { get; set; }
        public List<LinkParagraphEntity> LinkParagraphs { get; set; }
        public List<AudioEntity> Audios { get; set; }
    }
}
