using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Schemas
{
    /// <summary>
    /// letter of word
    /// حروف
    /// </summary>
    public class WordLetterSchema
    {
        public long Id { get; set; }
        public char Value { get; set; }
        public long WordId { get; set; }
    }
}
