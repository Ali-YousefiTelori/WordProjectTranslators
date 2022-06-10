using Translators.Database.Entities;

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

        public static LanguageEntity SearchLanguage = new LanguageEntity() { Code = "search", Name = "Search Language" };
        public ParagraphEntity GetParagraph(LanguageEntity language, CatalogEntity catalog)
        {
            int index = 0;
            var mainWords = UnSpaceArabic(text).Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x =>
            {
                var word = GetWord(index, language, x);
                index++;
                return word;
            }).Where(x => x.Values.All(v => !string.IsNullOrEmpty(v.Value.Trim()))).ToList();
            index = 0;
            mainWords.AddRange(UnSpaceArabic(text).Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x =>
            {
                var word = GetSimpleWord(index, x);
                index++;
                return word;
            }).Where(x => x.Values.All(v => !string.IsNullOrEmpty(v.Value.Trim()))));
            return new ParagraphEntity()
            {
                Number = numberInSurah,
                AnotherValue = $"{number}_{juz}_{manzil}_{page}_{pageInSurah}_{ruku}_{hizbQuarter}_{sajda}",
                Words = mainWords,
                Catalog = catalog
            };
        }

        WordEntity GetWord(int index, LanguageEntity language, string value)
        {
            return new WordEntity()
            {
                Index = index,
                Values = new List<ValueEntity>()
                {
                    new ValueEntity()
                    {
                        Language = language,
                        IsMain = true,
                        Value = SpaceArabic(Clean(value))
                    }
                }
            };
        }

        static string SearchChars = "اآبتثجچحخدذرزسشصضطظعغفقکلمنوهیكڪىيٱءإئۀةؤأ";
        WordEntity GetSimpleWord(int index, string value)
        {
            var word = new WordEntity()
            {
                Index = index,
                Values = new List<ValueEntity>()
                {
                    new ValueEntity()
                    {
                        Language = SearchLanguage,
                        IsMain = false,
                        Value = new string(Clean(value).Where(x => SearchChars.Contains(x)).ToArray())
                        .Replace("ٱ", "ا").Replace("آ", "ا").Replace("إ", "ا").Replace("أ", "ا").Replace("ء", "").Replace("ؤ", "و").Replace("ة", "ه").Replace("ۀ", "ه").Replace('ك', 'ک').Replace('ڪ', 'ک').Replace('ئ', 'ی').Replace('ي', 'ی').Replace('ى', 'ی')
                    }
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

        public CategoryEntity GetBook(LanguageEntity language)
        {
            return new CategoryEntity()
            {
                Names = new List<ValueEntity>()
                {
                    new ValueEntity()
                    {
                        IsMain = true,
                        Language = language,
                        Value = "القرآن"
                    }
                },
                Books = new List<BookEntity>()
                {
                    new BookEntity()
                    {
                        Names = new List<ValueEntity>()
                        {
                            new ValueEntity
                            {
                                IsMain = true,
                                Value = "القرآن الكريم",
                                Language = language
                            }
                        },
                        Catalogs = data.surahs.Select(x=> x.GetCatalog(language)).ToList()
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

        public CatalogEntity GetCatalog(LanguageEntity language)
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
            var catalog = new CatalogEntity()
            {
                Number = number,
                StartPageNumber = minPageNumber,
                Names = new List<ValueEntity>()
                {
                    new ValueEntity()
                    {
                        IsMain = true,
                        Language = language,
                        Value = name,
                    }
                }
            };
            catalog.Pages = ayahs.GroupBy(x => x.page).Select(x => new PageEntity()
            {
                Number = x.Key,
                Paragraphs = x.Select(i => i.GetParagraph(language, catalog)).ToList()
            }).ToList();
            return catalog;
        }
    }
}
