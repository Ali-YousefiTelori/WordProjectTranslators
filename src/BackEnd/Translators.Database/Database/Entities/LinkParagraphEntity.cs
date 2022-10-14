using Translators.Database.Entities.Authentications;

namespace Translators.Database.Entities
{
    /// <summary>
    /// ریفرنس هایی که کتاب ها به یکدیگر می دهند لینک محسوب می شود
    /// </summary>
    public class LinkParagraphEntity
    {
        public long UserId { get; set; }

        public long LinkGroupId { get; set; }
        public long ParagraphId { get; set; }

        public ParagraphEntity Paragraph { get; set; }
        public LinkGroupEntity LinkGroup { get; set; }

        public UserEntity User { get; set; }
    }
}