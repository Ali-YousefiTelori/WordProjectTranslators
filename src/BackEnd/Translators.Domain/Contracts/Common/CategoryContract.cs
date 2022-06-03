using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translators.Contracts.Common;

namespace Translators.Contracts.Common
{
    /// <summary>
    /// category of books
    /// </summary>
    public class CategoryContract
    {
        public long Id { get; set; }

        public long NameId { get; set; }
        public LanguageValueContract Name { get; set; }

        public List<BookContract> Books { get; set; }
    }
}
