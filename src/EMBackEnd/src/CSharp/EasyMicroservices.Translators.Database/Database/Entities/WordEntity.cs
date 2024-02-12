using System.Collections.Generic;
using Translators.Schemas;

namespace Translators.Database.Entities
{
    /// <summary>
    /// word of paragraph
    /// </summary>
    public class WordEntity : WordSchema
    {
        public List<ValueEntity> Values { get; set; }
        public ParagraphEntity Paragraph { get; set; }
        public List<WordLetterEntity> WordLetters { get; set; }
        public List<WordRootEntity> WordRoots { get; set; }
    }
}
