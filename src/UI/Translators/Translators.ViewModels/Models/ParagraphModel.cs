using SignalGo.Shared.Models;
using System.Linq;
using Translators.Contracts.Common;

namespace Translators.Models
{
    public class ParagraphBaseModel : NotifyPropertyChangedBase
    {
        public long Id { get; set; }
        public string DisplayName { get; set; }
        public string MainValue { get; set; }
        public string TranslatedValue { get; set; }
        public bool HasLink { get; set; }
        public long BookId { get; set; }
        public long PageNumber { get; set; }
        public long CatalogId { get; set; }
        public bool IsEven { get; set; }

        bool _IsSelected;
        public bool IsSelected
        {
            get => _IsSelected;
            set
            {
                _IsSelected = value;
                OnPropertyChanged(nameof(IsSelected));
                OnPropertyChanged(nameof(MeOnChanged));
            }
        }

        public ParagraphBaseModel MeOnChanged
        {
            get
            {
                return this;
            }
        }
    }

    public class ParagraphModel : ParagraphBaseModel
    {
        public long Number { get; set; }
        public static ParagraphModel Map(ParagraphContract paragraphContract)
        {
            return new ParagraphModel()
            {
                Id = paragraphContract.Id,
                HasLink = paragraphContract.HasLink,
                CatalogId = paragraphContract.CatalogId,
                Number = paragraphContract.Number,
                MainValue = string.Join(" ", paragraphContract.Words.OrderBy(x => x.Index).SelectMany(x => x.Values).Where(x => x.IsMain).Select(x => x.Value)),
                TranslatedValue = string.Join(" ", paragraphContract.Words.OrderBy(x => x.Index).SelectMany(x => x.Values).Where(x => !x.IsMain && x.Language.Code == "fa-ir").Select(x => x.Value)),
                BookId = paragraphContract.BookId,
                PageNumber = paragraphContract.PageNumber
            };
        }
    }
}
