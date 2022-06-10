namespace Translators.Database.Entities
{
    /// <summary>
    /// translate entity
    /// </summary>
    public class ValueEntity
    {
        public long Id { get; set; }
        /// <summary>
        /// is main of the value or false its translated
        /// </summary>
        public bool IsMain { get; set; }
        public string Value { get; set; }

        public long LanguageId { get; set; }
        public LanguageEntity Language { get; set; }

        /// <summary>
        /// در صورتی که مترجم مشخص نباشد برای این است که این آیتم یا متن اصلی است یا متن کلید شده برای جستجو می باشد
        /// یا اینکه نام مترجمین هست
        /// </summary>
        public long? TranslatorId { get; set; }
        public TranslatorEntity Translator { get; set; }

        public long? TranslatorNameId { get; set; }
        public TranslatorEntity TranslatorName { get; set; }

        public long? BookNameId { get; set; }
        public BookEntity BookName { get; set; }

        public long? CategoryNameId { get; set; }
        public CategoryEntity Category { get; set; }

        public long? CatalogNameId { get; set; }
        public CatalogEntity Catalog { get; set; }

        public long? WordValueId { get; set; }
        public WordEntity Word { get; set; }
    }
}
