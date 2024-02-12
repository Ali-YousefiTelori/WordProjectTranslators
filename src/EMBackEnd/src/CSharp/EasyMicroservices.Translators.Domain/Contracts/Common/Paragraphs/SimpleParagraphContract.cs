using System.Collections.Generic;
using System.Linq;
using Translators.Contracts.Common.Files;
using Translators.Contracts.Common.Words;

namespace Translators.Contracts.Common.Paragraphs
{
    public class SimpleParagraphContract
    {
        public long Id { get; set; }
        public long Number { get; set; }
        public bool HasLink { get; set; }

        public List<SimpleWordContract> MainWords { get; set; }
        public string TranslatedValue { get; set; }

        public List<SimpleFileContract> AudioFiles { get; set; }

        public override string ToString()
        {
            return string.Join(" ", MainWords.Select(x => x.Value));
        }
    }
}
