using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Contracts.Common
{
    /// <summary>
    /// catalog of any books
    /// sura in queran
    /// </summary>
    public class CatalogContract
    {
        public long Id { get; set; }

        public int Number { get; set; }
        public int StartPageNumber { get; set; }

        public long NameId { get; set; }
        public LanguageValueContract Name { get; set; }

        public long BookId { get; set; }

        public List<PageContract> Pages { get; set; }

        public override string ToString()
        {
            return Name.Value;
        }
    }
}
