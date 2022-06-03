using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Contracts.Common
{
    /// <summary>
    /// paragraph of a book page
    /// verses
    /// </summary>
    public class ParagraphContract
    {
        public long Id { get; set; }
        public long Number { get; set; }
        public string AnotherValue { get; set; }

        public long PageId { get; set; }

        public List<WordContract> Words { get; set; }

        public override string ToString()
        {
            return string.Join(' ', Words.Select(x => x.Value.Value)); 
        }
    }
}
