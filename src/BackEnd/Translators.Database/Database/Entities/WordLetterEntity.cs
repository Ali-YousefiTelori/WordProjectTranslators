using Translators.Schemas;

namespace Translators.Database.Entities
{
    /// <summary>
    /// letter of word
    /// حروف
    /// </summary>
    public class WordLetterEntity : WordLetterSchema
    {
        public WordEntity Word { get; set; }
    }
}
