using System.Collections.Generic;
using Translators.Schemas.Bases;

namespace Translators.Contracts.Common
{
    /// <summary>
    /// real books
    /// </summary>
    public class BookContract : BookSchemaBase
    {
        public List<ValueContract> Names { get; set; }
        public List<CatalogContract> Catalogs { get; set; }
    }
}
