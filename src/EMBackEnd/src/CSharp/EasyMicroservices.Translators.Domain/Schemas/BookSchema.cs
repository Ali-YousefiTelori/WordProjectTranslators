using System.Collections.Generic;
using Translators.Schemas.Bases;

namespace Translators.Schemas
{
    /// <summary>
    /// real books
    /// </summary>
    public class BookSchema : BookSchemaBase
    {
        public List<ValueSchema> Names { get; set; }
    }
}
