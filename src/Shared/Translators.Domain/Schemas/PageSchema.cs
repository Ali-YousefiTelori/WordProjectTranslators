namespace Translators.Schemas
{
    /// <summary>
    /// page of book
    /// </summary>
    public class PageSchema
    {
        public long Id { get; set; }
        public long Number { get; set; }
        public long CatalogId { get; set; }
    }
}
