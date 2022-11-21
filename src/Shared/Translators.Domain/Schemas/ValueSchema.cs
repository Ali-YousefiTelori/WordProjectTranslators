using Translators.Contracts.Common;

namespace Translators.Schemas
{
    /// <summary>
    /// translate entity
    /// </summary>
    public class ValueSchema
    {
        public long Id { get; set; }
        /// <summary>
        /// is main of the value or false its translated
        /// </summary>
        public bool IsMain { get; set; }
        public bool IsTransliteration { get; set; }
        public string Value { get; set; }
        public string SearchValue { get; set; }
        public long LanguageId { get; set; }
        /// <summary>
        /// در صورتی که مترجم مشخص نباشد برای این است که این آیتم یا متن اصلی است یا متن کلید شده برای جستجو می باشد
        /// یا اینکه نام مترجمین هست
        /// </summary>
        public long? TranslatorId { get; set; }
        public long? TranslatorNameId { get; set; }
        public long? BookNameId { get; set; }
        public long? CategoryNameId { get; set; }
        public long? CatalogNameId { get; set; }
        public long? WordValueId { get; set; }

        public static string GetKey(ValueSchema  valueSchema)
        {
            return $"{valueSchema.IsMain}_{valueSchema.LanguageId}_{valueSchema.TranslatorId}_{valueSchema.IsTransliteration}";
        }
    }
}
