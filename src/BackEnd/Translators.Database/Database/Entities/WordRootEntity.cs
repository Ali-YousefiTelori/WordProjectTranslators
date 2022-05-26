using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Database.Entities
{
    /// <summary>
    /// ریشه
    /// root of word
    /// </summary>
    public class WordRootEntity
    {
        public long Id { get; set; }
        public string Value { get; set; }

        public long WordId { get; set; }
        public WordEntity Word { get; set; }
    }
}
