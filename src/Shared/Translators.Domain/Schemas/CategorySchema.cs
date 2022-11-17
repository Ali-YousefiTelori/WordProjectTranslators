using System.Collections.Generic;
using Translators.Schemas.Bases;

namespace Translators.Schemas
{
    public class CategorySchema : CategorySchemaBase
    {
        public List<ValueSchema> Names { get; set; }
    }
}
