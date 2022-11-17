using Translators.Schemas;

namespace Translators.Contracts.Common
{
    public class ValueContract : ValueSchema
    {
        public LanguageContract Language { get; set; }
        public TranslatorContract Translator { get; set; }
    }
}
