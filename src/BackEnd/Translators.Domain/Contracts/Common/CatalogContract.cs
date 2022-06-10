namespace Translators.Contracts.Common
{
    /// <summary>
    /// catalog of any books
    /// sura in queran
    /// </summary>
    public class CatalogContract
    {
        public long Id { get; set; }

        public int Number { get; set; }
        public int StartPageNumber { get; set; }

        public List<ValueContract> Names { get; set; }

        public long BookId { get; set; }

        public List<PageContract> Pages { get; set; }

        public override string ToString()
        {
            return string.Join(",", Names.Select(x => x.Value));
        }
    }
}
