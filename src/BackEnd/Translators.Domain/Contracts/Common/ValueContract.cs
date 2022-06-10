using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Contracts.Common
{
    public class ValueContract
    {
        public long Id { get; set; }
        /// <summary>
        /// is main of the value or false its translated
        /// </summary>
        public bool IsMain { get; set; }
        public string Value { get; set; }

        public long LanguageId { get; set; }
        public LanguageContract Language { get; set; }

        public long? TranslatorId { get; set; }
        public TranslatorContract Translator { get; set; }
    }
}
