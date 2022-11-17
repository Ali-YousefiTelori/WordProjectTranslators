using System.Collections.Generic;
using Translators.Schemas;
using Translators.Schemas.Bases;

namespace Translators.Contracts.Common
{
    public class TranslatorContract : TranslatorSchemaBase
    {
        public List<ValueContract> Names { get; set; }
    }
}
