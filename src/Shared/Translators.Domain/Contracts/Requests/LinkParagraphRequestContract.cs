using System.Collections.Generic;

namespace Translators.Contracts.Requests
{
    public class LinkParagraphRequestContract
    {
        public string Title { get; set; }
        public List<long> FromParagraphIds { get; set; }
        public List<long> ToParagraphIds { get; set; }
    }
}
