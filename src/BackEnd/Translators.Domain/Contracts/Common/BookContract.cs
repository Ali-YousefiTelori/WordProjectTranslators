using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Contracts.Common
{
    /// <summary>
    /// real books
    /// </summary>
    public class BookContract
    {
        public long Id { get; set; }

        public List<ValueContract> Names { get; set; }

        public long CategoryId { get; set; }

        public List<CatalogContract> Catalogs { get; set; }
    }
}
