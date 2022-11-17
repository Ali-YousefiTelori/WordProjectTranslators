using Translators.Schemas;

namespace Translators.Database.Entities
{
    public class LinkGroupEntity : LinkGroupSchema
    {
        public List<LinkParagraphEntity> LinkParagraphs { get; set; }
    }
}
