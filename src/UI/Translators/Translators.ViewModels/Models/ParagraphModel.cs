using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translators.Contracts.Common;

namespace Translators.Models
{
    public class ParagraphModel
    {
        public string MainValue { get; set; }
        public string TranslatedValue { get; set; }

        public static implicit operator ParagraphModel(ParagraphContract paragraphContract)
        {
            return new ParagraphModel()
            {
                MainValue = string.Join(" ", paragraphContract.Words.Where(x => x.Value.IsMain).OrderBy(x => x.Index).Select(x => x.Value.Value)),
                TranslatedValue = string.Join(" ", paragraphContract.Words.Where(x => !x.Value.IsMain && x.Value.Language.Code != "").OrderBy(x => x.Index).Select(x => x.Value.Value)),
            };
        }
    }
}
