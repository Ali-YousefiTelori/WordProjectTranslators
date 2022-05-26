using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Database.Entities
{
    /// <summary>
    /// letter of word
    /// حروف
    /// </summary>
    public class WordLetterEntity
    {
        public long Id { get; set; }
        public char Value { get; set; }

        public long WordId { get; set; }
        public WordEntity Word { get; set; }
    }
}
