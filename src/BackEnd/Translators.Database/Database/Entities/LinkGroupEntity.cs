using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Database.Entities
{
    public class LinkGroupEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public List<LinkParagraphEntity> LinkParagraphs { get; set; }
    }
}
