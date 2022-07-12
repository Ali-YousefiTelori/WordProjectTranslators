using Translators.Validations;

namespace Translators.Contracts.Requests
{
    public class LinkParagraphRequestContract
    {
        [NumberValidation]
        public string Title { get; set; }
        [NumberValidation]
        public long FromParagraphId { get; set; }
        [NumberValidation]
        public long ToParagraphId { get; set; }
    }
}
