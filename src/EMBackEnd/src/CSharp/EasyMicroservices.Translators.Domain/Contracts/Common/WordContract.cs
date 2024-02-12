using System.Collections.Generic;
using System.Linq;
using Translators.Schemas;

namespace Translators.Contracts.Common
{
    /// <summary>
    /// word of paragraph
    /// </summary>
    public class WordContract : WordSchema
    {
        public List<ValueContract> Values { get; set; }
        public List<WordLetterContract> WordLetters { get; set; }
        public List<WordRootContract> WordRoots { get; set; }

        public override string ToString()
        {
            return string.Join(",", Values.Select(x => x.Value));
        }
    }
}
