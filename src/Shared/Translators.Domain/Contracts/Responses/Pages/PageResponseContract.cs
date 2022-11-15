using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Contracts.Common.Files;
using Translators.Contracts.Common.Paragraphs;

namespace Translators.Contracts.Responses.Pages
{
    public class PageResponseContract
    {
        public string CatalogName { get; set; }
        public List<LanguageContract> Languages { get; set; }
        public List<SimpleParagraphContract> Paragraphs { get; set; }
        public List<SimpleFileContract> AudioFiles { get; set; }
    }
}
