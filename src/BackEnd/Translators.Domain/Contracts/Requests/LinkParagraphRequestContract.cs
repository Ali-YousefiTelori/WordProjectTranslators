using Translators.Validations;

namespace Translators.Contracts.Requests
{
    public class LinkParagraphRequestContract
    {
        [NumberValidation]
        public string Title { get; set; }
        [NumberValidation]
        public List<long> FromParagraphIds { get; set; }
        [NumberValidation]
        public List<long> ToParagraphIds { get; set; }
    }
}
