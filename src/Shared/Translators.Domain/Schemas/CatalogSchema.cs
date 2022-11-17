using System.Collections.Generic;
using Translators.Schemas.Bases;

namespace Translators.Schemas
{
    public class CatalogSchema : CatalogSchemaBase
    {
        public List<ValueSchema> Names { get; set; }
    }
}
