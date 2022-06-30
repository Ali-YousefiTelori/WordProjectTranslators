using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Contracts.Requests
{
    public class AdvancedSearchFilterRequestContract
    {
        public string Search { get; set; }
        public bool DoSearchInQuran { get; set; }
        public bool DoSearchInTorat { get; set; }
        public bool DoSearchInEnjil { get; set; }
    }
}
