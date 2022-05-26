using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Database.Entities
{
    /// <summary>
    /// word of paragraph
    /// </summary>
    public class WordEntity
    {
        public long Id { get; set; }
        public int Index { get; set; }

        public long ValueId { get; set; }
        public LanguageValueEntity Value { get; set; }

        public long ParagraphId { get; set; }
        public ParagraphEntity Paragraph { get; set; }

        public List<WordLetterEntity> WordLetters { get; set; }
        public List<WordRootEntity> WordRoots { get; set; }
    }
}
