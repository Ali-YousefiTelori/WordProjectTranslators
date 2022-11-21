namespace Translators.Schemas
{
    /// <summary>
    /// paragraph of a book page
    /// </summary>
    public class ParagraphSchema
    {
        public long Id { get; set; }
        public long Number { get; set; }
        public string AnotherValue { get; set; }
        public long PageId { get; set; }
        public long CatalogId { get; set; }

        public static string GetKey(ParagraphSchema paragraphSchema)
        {
            return $"{paragraphSchema.CatalogId}";
        }
    }
}
