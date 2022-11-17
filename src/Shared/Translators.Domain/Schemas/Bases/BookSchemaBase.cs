namespace Translators.Schemas.Bases
{
    /// <summary>
    /// real books
    /// </summary>
    public class BookSchemaBase
    {
        public long Id { get; set; }
        public bool IsHidden { get; set; }
        public long CategoryId { get; set; }
    }
}
