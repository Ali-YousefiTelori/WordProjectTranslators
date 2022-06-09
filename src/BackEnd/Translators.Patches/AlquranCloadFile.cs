using Translators.Contracts.Common;

namespace Translators.Patches
{
    /// <summary>
    /// cloned from https://alquran.cloud/api
    /// </summary>
    public class Ayah
    {
        public int number { get; set; }
        public string text { get; set; }
        public int numberInSurah { get; set; }
        public int juz { get; set; }
        public int manzil { get; set; }
        public int page { get; set; }
        public int pageInSurah { get; set; } = 1;
        public int ruku { get; set; }
        public int hizbQuarter { get; set; }
        public object sajda { get; set; }

        public static long SearchLanguageId;
        public ParagraphContract GetParagraph(long languageId)
        {
            int index = 0;
            var mainWords = UnSpaceArabic(text).Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x =>
            {
                var word = GetWord(index, languageId, x);
                index++;
                return word;
            }).Where(x => !string.IsNullOrEmpty(x.Value.Value.Trim())).ToList();
            index = 0;
            mainWords.AddRange(UnSpaceArabic(text).Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x =>
            {
                var word = GetSimpleWord(index, x);
                index++;
                return word;
            }).Where(x => !string.IsNullOrEmpty(x.Value.Value.Trim())));
            return new ParagraphContract()
            {
                Number = numberInSurah,
                AnotherValue = $"{number}_{juz}_{manzil}_{page}_{pageInSurah}_{ruku}_{hizbQuarter}_{sajda}",
                Words = mainWords
            };
        }

        WordContract GetWord(int index, long languageId, string value)
        {
            return new WordContract()
            {
                Index = index,
                Value = new LanguageValueContract()
                {
                    LanguageId = languageId,
                    IsMain = true,
                    Value = SpaceArabic(Clean(value))
                }
            };
        }

        static string SearchChars = "اآبتثجچحخدذرزسشصضطظعغفقکلمنوهیكڪىيٱءإئۀةؤأ";
        WordContract GetSimpleWord(int index, string value)
        {
            var word = new WordContract()
            {
                Index = index,
                Value = new LanguageValueContract()
                {
                    LanguageId = SearchLanguageId,
                    IsMain = false,
                    Value = new string(Clean(value).Where(x => SearchChars.Contains(x)).ToArray())
                    .Replace("ٱ", "ا").Replace("آ", "ا").Replace("إ", "ا").Replace("أ", "ا").Replace("ء", "").Replace("ؤ", "و").Replace("ة", "ه").Replace("ۀ", "ه").Replace('ك', 'ک').Replace('ڪ', 'ک').Replace('ئ', 'ی').Replace('ي', 'ی').Replace('ى', 'ی')
                }
            };
            return word;
        }

        string UnSpaceArabic(string text)
        {
            return text.Replace(" ۛ", "ۛ").Replace(" ۖ", "ۖ").Replace(" ۗ", "ۗ").Replace(" ۚ", "ۚ").Replace(" ۙ", "ۙ").Replace(" ۘ", "").Replace(" ۜ", "");
        }

        string SpaceArabic(string text)
        {
            return text.Replace("ۛ", " ۛ").Replace("ۖ", " ۖ").Replace("ۗ", " ۗ").Replace("ۚ", " ۚ").Replace("ۙ", " ۙ").Replace("ۘ", "").Replace("ۜ", " ۜ");
        }

        string Clean(string text)
        {
            return text.Replace("۞", "").Replace("۩", "");
        }
    }

    public class Data
    {
        public List<Surah> surahs { get; set; }
        public Edition edition { get; set; }
    }

    public class Edition
    {
        public string identifier { get; set; }
        public string language { get; set; }
        public string name { get; set; }
        public string englishName { get; set; }
        public string format { get; set; }
        public string type { get; set; }
    }

    public class AlquranCloadRoot
    {
        public int code { get; set; }
        public string status { get; set; }
        public Data data { get; set; }

        public CategoryContract GetBook(long languageId)
        {
            return new CategoryContract()
            {
                Name = new LanguageValueContract()
                {
                    IsMain = true,
                    LanguageId = languageId,
                    Value = "القرآن"
                },
                Books = new List<BookContract>()
                {
                    new BookContract()
                    {
                        Name = new LanguageValueContract
                        {
                            IsMain = true,
                            Value = "القرآن الكريم",
                            LanguageId = languageId
                        },
                        Catalogs = data.surahs.Select(x=> x.GetCatalog(languageId)).ToList()
                    }
                }
            };
        }
    }

    public class Surah
    {
        public int number { get; set; }
        public string name { get; set; }
        public string englishName { get; set; }
        public string englishNameTranslation { get; set; }
        public string revelationType { get; set; }
        public List<Ayah> ayahs { get; set; }

        public CatalogContract GetCatalog(long languageId)
        {
            var pageGroup = ayahs.GroupBy(x => x.page);
            var minPageNumber = ayahs.GroupBy(x => x.page).Min(x => x.Key);
            if (number > 1)
            {
                ayahs.First().text = ayahs.First().text.Replace("بِسْمِ ٱللَّهِ ٱلرَّحْمَٰنِ ٱلرَّحِيمِ ", "").Replace("بِّسْمِ ٱللَّهِ ٱلرَّحْمَٰنِ ٱلرَّحِيمِ ", "");
            }
            foreach (var group in pageGroup)
            {
                foreach (var ayah in group)
                {
                    ayah.pageInSurah = ayah.page - minPageNumber + 1;
                }
            }
            return new CatalogContract()
            {
                Number = number,
                StartPageNumber = minPageNumber,
                Name = new LanguageValueContract()
                {
                    IsMain = true,
                    LanguageId = languageId,
                    Value = name,
                },
                Pages = ayahs.GroupBy(x => x.page).Select(x => new PageContract()
                {
                    Number = x.Key,
                    Paragraphs = x.Select(i => i.GetParagraph(languageId)).ToList()
                }).ToList()
            };
        }
    }
}
