using Translators.Schemas;

namespace Translators.Database.Entities
{
    /// <summary>
    /// translate entity
    /// </summary>
    public class ValueEntity : ValueSchema
    {
        public LanguageEntity Language { get; set; }
        public TranslatorEntity Translator { get; set; }
        public TranslatorEntity TranslatorName { get; set; }
        public BookEntity BookName { get; set; }
        public CategoryEntity Category { get; set; }
        public CatalogEntity Catalog { get; set; }
        public WordEntity Word { get; set; }
    }
}
