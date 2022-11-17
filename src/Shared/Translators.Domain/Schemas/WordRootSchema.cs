using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Schemas
{
    /// <summary>
    /// ریشه
    /// root of word
    /// </summary>
    public class WordRootSchema
    {
        public long Id { get; set; }
        public string Value { get; set; }
        public long WordId { get; set; }
    }
}
