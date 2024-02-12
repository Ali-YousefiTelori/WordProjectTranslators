using Translators.Schemas;

namespace Translators.Database.Entities
{
    /// <summary>
    /// ریشه
    /// root of word
    /// </summary>
    public class WordRootEntity : WordRootSchema
    {
        public WordEntity Word { get; set; }
    }
}
