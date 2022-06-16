using System.Linq;
using Translators.Contracts.Common;

namespace Translators.Models
{
    public class ParagraphModel
    {
        public long Number { get; set; }
        public string MainValue { get; set; }
        public string TranslatedValue { get; set; }
        public long CatalogId { get; set; }
        public static ParagraphModel Map(ParagraphContract paragraphContract)
        {
            return new ParagraphModel()
            {
                CatalogId = paragraphContract.CatalogId,
                Number = paragraphContract.Number,
                MainValue = string.Join(" ", paragraphContract.Words.OrderBy(x => x.Index).SelectMany(x => x.Values).Where(x => x.IsMain).Select(x => x.Value)),
                TranslatedValue = string.Join(" ", paragraphContract.Words.OrderBy(x => x.Index).SelectMany(x => x.Values).Where(x => !x.IsMain && x.Language.Code == "fa-ir").Select(x => x.Value))
            };
        }
    }
}
