using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Database.Entities
{
    public class AudioEntity
    {
        public long Id { get; set; }

        /// <summary>
        /// is main of the value or false its translated
        /// </summary>
        public bool IsMain { get; set; }
        public string FileName { get; set; }
        public byte[] Data { get; set; }
        public string Password { get; set; }

        public long? PageId { get; set; }
        public PageEntity Page { get; set; }

        public long? LanguageId { get; set; }
        public LanguageEntity Language { get; set; }

        public long? ParagraphId { get; set; }
        public ParagraphEntity Paragraph { get; set; }
        public long? AudioReaderId { get; set; }
        public AudioReaderEntity AudioReader { get; set; }

        /// <summary>
        /// در صورتی که مترجم مشخص نباشد برای این است که این آیتم یا متن اصلی است یا متن کلید شده برای جستجو می باشد
        /// یا اینکه نام مترجمین هست
        /// </summary>
        public long? TranslatorId { get; set; }
        public TranslatorEntity Translator { get; set; }
    }
}
