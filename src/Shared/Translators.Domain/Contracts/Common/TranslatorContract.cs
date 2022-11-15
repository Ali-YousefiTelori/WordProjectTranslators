using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Contracts.Common
{
    public class TranslatorContract
    {
        public long Id { get; set; }
        /// <summary>
        /// نام مترجم
        /// </summary>
        public List<ValueContract> Names { get; set; }
    }
}
