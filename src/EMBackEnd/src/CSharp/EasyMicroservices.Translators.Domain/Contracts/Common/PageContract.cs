using System.Collections.Generic;
using Translators.Schemas;

namespace Translators.Contracts.Common
{
    /// <summary>
    /// page of book
    /// </summary>
    public class PageContract : PageSchema
    {
        public List<ValueContract> CatalogNames { get; set; }
        public long BookId { get; set; }

        public List<ParagraphContract> Paragraphs { get; set; }
        public List<AudioFileContract> AudioFiles { get; set; }
    }
}
