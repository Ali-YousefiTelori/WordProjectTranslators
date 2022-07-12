using Translators.Database.Entities.Authentications;

namespace Translators.Database.Entities
{
    public class LinkParagraphEntity
    {
        public long UserId { get; set; }

        public long LinkGroupId { get; set; }
        public long FromParagraphId { get; set; }
        public long ToParagraphId { get; set; }

        public ParagraphEntity FromParagraph { get; set; }
        public ParagraphEntity ToParagraph { get; set; }
        public LinkGroupEntity LinkGroup { get; set; }

        public UserEntity User { get; set; }
    }
}