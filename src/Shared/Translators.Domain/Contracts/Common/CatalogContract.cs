using System.Collections.Generic;
using System.Linq;
using Translators.Schemas.Bases;

namespace Translators.Contracts.Common
{
    /// <summary>
    /// catalog of any books
    /// sura in queran
    /// </summary>
    public class CatalogContract : CatalogSchemaBase
    {
        public List<ValueContract> Names { get; set; }
        public List<PageContract> Pages { get; set; }
        public List<ValueContract> BookNames { get; set; }

        public override string ToString()
        {
            return string.Join(",", Names.Select(x => x.Value));
        }
    }
}
