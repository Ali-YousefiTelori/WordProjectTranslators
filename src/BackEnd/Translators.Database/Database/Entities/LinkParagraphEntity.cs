using Translators.Database.Entities.Authentications;
using Translators.Schemas;

namespace Translators.Database.Entities
{
    /// <summary>
    /// ریفرنس هایی که کتاب ها به یکدیگر می دهند لینک محسوب می شود
    /// </summary>
    public class LinkParagraphEntity : LinkParagraphSchema
    {
        public ParagraphEntity Paragraph { get; set; }
        public LinkGroupEntity LinkGroup { get; set; }
        public UserEntity User { get; set; }
    }
}