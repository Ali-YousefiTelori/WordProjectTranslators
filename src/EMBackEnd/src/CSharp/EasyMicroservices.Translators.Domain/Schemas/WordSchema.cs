using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Schemas
{
    /// <summary>
    /// word of paragraph
    /// </summary>
    public class WordSchema
    {
        public long Id { get; set; }
        public int Index { get; set; }
        public long ParagraphId { get; set; }
    }
}
