﻿namespace Translators.Contracts.Common
{
    public class AudioFileContract : FileContract
    {
        /// <summary>
        /// is main of the value or false its translated
        /// </summary>
        public bool IsMain { get; set; }
        public string FileName { get; set; }
        public long DurationTicks { get; set; }
        public long? PageId { get; set; }
        public long? LanguageId { get; set; }
        /// <summary>
        /// در صورتی که مترجم مشخص نباشد برای این است که این آیتم یا متن اصلی است یا متن کلید شده برای جستجو می باشد
        /// یا اینکه نام مترجمین هست
        /// </summary>
        public long? TranslatorId { get; set; }
        public long? ParagraphId { get; set; }
        public long? AudioReaderId { get; set; }

        public static string GetKey(AudioFileContract audioFile)
        {
            return $"{audioFile.IsMain}_{audioFile.LanguageId}_{audioFile.TranslatorId}_{audioFile.AudioReaderId}";
        }
    }
}
