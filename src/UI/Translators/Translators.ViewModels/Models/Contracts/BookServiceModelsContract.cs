using System;
using System.Collections.Generic;
using System.Text;
using Translators.Contracts.Common;

namespace Translators.Models.Contracts
{
    public class BookServiceModelsContract
    {
        public List<CategoryContract> Categories { get; set; }
        public List<BookContract> Books { get; set; }
    }
}
