using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Contracts.Common
{
    /// <summary>
    /// word of paragraph
    /// </summary>
    public class WordContract
    {
        public long Id { get; set; }
        public int Index { get; set; }

        public long ValueId { get; set; }
        public LanguageValueContract Value { get; set; }

        public long ParagraphId { get; set; }

        public List<WordLetterContract> WordLetters { get; set; }
        public List<WordRootContract> WordRoots { get; set; }

        public override string ToString()
        {
            return Value.Value;
        }
    }
}
