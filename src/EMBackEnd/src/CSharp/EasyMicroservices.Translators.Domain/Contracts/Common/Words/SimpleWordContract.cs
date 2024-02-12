using Translators.Contracts.Common.Values;

namespace Translators.Contracts.Common.Words
{
    public class SimpleWordContract
    {
        public long Id { get; set; }
        public int Index { get; set; }
        public bool IsTransliteration { get; set; }
        public string Value { get; set; }
        public long LanguageId { get; set; }
    }
}
