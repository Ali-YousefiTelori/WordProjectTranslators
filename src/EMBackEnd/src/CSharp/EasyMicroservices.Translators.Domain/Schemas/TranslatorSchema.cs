using System.Collections.Generic;
using Translators.Schemas.Bases;

namespace Translators.Schemas
{
    public class TranslatorSchema : TranslatorSchemaBase
    {
        public List<ValueSchema> Names { get; set; }
    }
}
