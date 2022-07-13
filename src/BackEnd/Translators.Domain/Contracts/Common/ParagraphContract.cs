namespace Translators.Contracts.Common
{
    /// <summary>
    /// paragraph of a book page
    /// verses
    /// </summary>
    public class ParagraphContract
    {
        public long Id { get; set; }
        public long Number { get; set; }
        public string AnotherValue { get; set; }
        public bool HasLink { get; set; }

        public long PageId { get; set; }
        public long CatalogId { get; set; }
        public long BookId { get; set; }
        public long PageNumber { get; set; }
        
        public List<WordContract> Words { get; set; }

        string mainValue = "";
        string translatedValue = "";
        public string GetMainSearchValue()
        {
            if (!string.IsNullOrEmpty(mainValue))
                return mainValue;
            mainValue = string.Join(' ', Words.SelectMany(w => w.Values).Where(v => v.IsMain).Select(x => x.SearchValue));
            return mainValue;
        }

        public string GetTranslatedSearchValue()
        {
            if (!string.IsNullOrEmpty(translatedValue))
                return translatedValue;
            translatedValue = string.Join(' ', Words.SelectMany(w => w.Values).Where(v => !v.IsMain).Select(x => x.SearchValue));
            return translatedValue;
        }

        public override string ToString()
        {
            return string.Join(' ', Words.Select(x => x.Values.Select(v => v.Value)));
        }
    }
}
