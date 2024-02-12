using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Translators.Database.Entities;

namespace Translators.Helpers
{
    public static class TextHelper
    {
        public static string SearchChars = "اآبپتثجچحخدذرزژسشصضطظعغفقکگلمنوهیكڪىيٱءإئۀةؤأ";

        public static string UnSpaceArabic(string text)
        {
            return text.Replace(" ۛ", "ۛ").Replace(" ۖ", "ۖ").Replace(" ۗ", "ۗ").Replace(" ۚ", "ۚ").Replace(" ۙ", "ۙ").Replace(" ۘ", "ۘ").Replace(" ۜ", "ۜ");
        }

        public static string SpaceArabic(string text)
        {
            return text.Replace("ۛ", " ۛ").Replace("ۖ", " ۖ").Replace("ۗ", " ۗ").Replace("ۚ", " ۚ").Replace("ۙ", " ۙ").Replace("ۘ", " ۘ").Replace("ۜ", " ۜ");
        }

        public static string CleanArabicChars(string text)
        {
            return text.Replace("ۛ", "").Replace("ۖ", "").Replace("ۗ", "").Replace("ۚ", "").Replace("ۙ", "").Replace("ۘ", "").Replace("ۜ", "").Replace("ِ", "").Replace("ُ", "").Replace("َ", "").Replace("ً", "").Replace("ٌ", "").Replace("ٍ", "").Replace("ّ", "").Replace("ِ", "").Replace("ُ", "").Replace("َ", "").Replace("ً", "").Replace("ٌ", "").Replace("ٍ", "").Replace("ّ", "");
        }

        public static string Clean(string text)
        {
            return text.Replace("۞", "").Replace("۩", "");
        }

        public static string FixArabicForSearch(string text)
        {
            return CleanArabicChars(text).Replace("ٱ", "ا").Replace("آ", "ا").Replace("إ", "ا").Replace("أ", "ا").Replace("ء", "").Replace("ؤ", "و").Replace("ة", "ه").Replace("ۀ", "ه").Replace('ك', 'ک').Replace('ڪ', 'ک').Replace('ئ', 'ی').Replace('ي', 'ی').Replace('ى', 'ی').Replace("‌", "");
        }

        /// <summary>
        /// extract numbers from text
        /// </summary>
        /// <param name="text">text to extract numbers</param>
        /// <returns>numbers extracted from text</returns>
        public static string ExtractNumbers(string text)
        {
            if (text == null)
                return text;
            return string.Join(string.Empty, Regex.Matches(text, @"\d+").OfType<Match>().Select(m => m.Value));
        }

        /// <summary>
        /// remove numbers from text
        /// </summary>
        /// <param name="text">text to remove numbers</param>
        /// <returns>numbers removed from text</returns>
        public static string RemoveNumbers(string text)
        {
            if (text == null)
                return text;
            return Regex.Replace(text, @"[\d-]", string.Empty);
        }

        /// <summary>
        /// extract integer from string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int ExtractInteger(string text)
        {
            if (text == null)
                return 0;
            text = ExtractNumbers(text);
            if (string.IsNullOrEmpty(text))
                return 0;
            return int.Parse(text);
        }

        public static ParagraphEntity GetParagraph(string text, LanguageEntity language, CatalogEntity catalog, bool isMain)
        {
            int index = 0;
            var mainWords = UnSpaceArabic(text).Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x =>
            {
                var word = GetWord(index, language, x, isMain);
                index++;
                return word;
            }).Where(x => x.Values.All(v => !string.IsNullOrEmpty(v.Value.Trim()))).ToList();

            return new ParagraphEntity()
            {
                Words = mainWords,
                Catalog = catalog
            };
        }

        static WordEntity GetWord(int index, LanguageEntity language, string value, bool isMain)
        {
            return new WordEntity()
            {
                Index = index,
                Values = new List<ValueEntity>()
                {
                    new ValueEntity()
                    {
                        Language = language,
                        IsMain = isMain,
                        Value = SpaceArabic(Clean(value)),
                        SearchValue = FixArabicForSearch(new string(Clean(value).Where(x => SearchChars.Contains(x)).ToArray()))
                    }
                }
            };
        }
    }
}
