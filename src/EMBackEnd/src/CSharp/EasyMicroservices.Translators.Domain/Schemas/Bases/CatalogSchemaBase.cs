namespace Translators.Schemas.Bases
{
    public class CatalogSchemaBase
    {
        public long Id { get; set; }
        public int Number { get; set; }
        public int StartPageNumber { get; set; }
        public long BookId { get; set; }
    }
}
