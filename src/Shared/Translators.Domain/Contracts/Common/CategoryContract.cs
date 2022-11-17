using System.Collections.Generic;
using Translators.Schemas.Bases;

namespace Translators.Contracts.Common
{
    /// <summary>
    /// category of books
    /// </summary>
    public class CategoryContract : CategorySchemaBase
    {
        public List<ValueContract> Names { get; set; }
        public List<BookContract> Books { get; set; }
    }
}
