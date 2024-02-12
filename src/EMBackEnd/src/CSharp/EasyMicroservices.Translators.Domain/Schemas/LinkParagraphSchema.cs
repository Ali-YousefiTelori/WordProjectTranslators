namespace Translators.Schemas
{
    /// <summary>
    /// ریفرنس هایی که کتاب ها به یکدیگر می دهند لینک محسوب می شود
    /// </summary>
    public class LinkParagraphSchema
    {
        public long UserId { get; set; }
        public long LinkGroupId { get; set; }
        public long ParagraphId { get; set; }
    }
}