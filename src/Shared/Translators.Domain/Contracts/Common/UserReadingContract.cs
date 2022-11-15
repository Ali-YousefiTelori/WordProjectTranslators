namespace Translators.Contracts.Common
{
    public class UserReadingContract
    {
        public string Name { get; set; }
        public string Title { get; set; }

        public long BookId { get; set; }
        public long CatalogId { get; set; }
        public long PageId { get; set; }
        public long CategoryId { get; set; }
        public int StartPageNumber { get; set; }
    }
}
