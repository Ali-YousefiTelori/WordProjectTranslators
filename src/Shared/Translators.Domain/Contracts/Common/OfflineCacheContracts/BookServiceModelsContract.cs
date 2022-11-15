using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Contracts.Common.OfflineCacheContracts
{
    public class BookServiceModelsContract
    {
        public List<CategoryContract> Categories { get; set; }
        public List<BookContract> Books { get; set; }
    }
}
