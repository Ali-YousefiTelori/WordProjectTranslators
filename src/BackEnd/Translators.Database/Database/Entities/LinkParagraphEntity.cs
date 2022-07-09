using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Database.Entities
{
    public class LinkParagraphEntity
    {
        public long LinkGroupId { get; set; }
        public long FromParagraphId { get; set; }
        public long ToParagraphId { get; set; }

        public ParagraphEntity FromParagraph { get; set; }
        public ParagraphEntity ToParagraph { get; set; }
        public LinkGroupEntity LinkGroup { get; set; }
    }
}