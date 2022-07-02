using Translators.Database.Entities;
using Translators.Helpers;

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
                Paragraphs = x.Select(i =>
                {
                    var result = TextHelper.GetParagraph(i.text, language, catalog, true);
                    result.Number = i.numberInSurah;
                    result.AnotherValue = $"{i.number}_{i.juz}_{i.manzil}_{i.page}_{i.pageInSurah}_{i.ruku}_{i.hizbQuarter}_{i.sajda}";
                    return result;
                }).ToList()
            }).ToList();
            return catalog;
        }
    }
}
