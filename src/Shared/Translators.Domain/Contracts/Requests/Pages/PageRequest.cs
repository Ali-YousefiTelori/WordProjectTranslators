using Translators.Validations;

namespace Translators.Contracts.Requests.Pages
{
    public class PageRequest
    {
        [NumberValidation]
        public long PageNumber { get; set; }

        [NumberValidation]
        public long BookId { get; set; }
    }
}
