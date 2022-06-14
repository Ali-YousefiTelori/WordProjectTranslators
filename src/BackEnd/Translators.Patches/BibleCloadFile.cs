using Translators.Database.Entities;

namespace Translators.Patches
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class BibleCloadAyat
    {
        public List<BibleCloadAyat> Ayat { get; set; }
        public long Number { get; set; }
        public BibleCloadAyeha Ayeha { get; set; }
    }

    public class BibleCloadAyeha
    {
        public BibleCloadLanguageText LanguageText { get; set; }
    }

    public class BibleCloadBooks
    {
        public List<BibleCloadKetab> Ketab { get; set; }
    }

    public class BibleCloadKetab
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public BibleCloadSureha Sureha { get; set; }
    }

    public class BibleCloadLanguageText
    {
        public string Language { get; set; }
        public string TeXt { get; set; }
    }

    public class BibleCloadMainBook
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public string LanguageID { get; set; }
        public BibleCloadBooks Books { get; set; }

        public CategoryEntity GetBook(LanguageEntity language, bool isMain)
        {
            int pageNumbers = 0;
            return new CategoryEntity()
            {
                Names = new List<ValueEntity>()
                {
                    new ValueEntity()
                    {
                        IsMain = isMain,
                        Language = language,
                        Value = Name
                    }
                },
                Books = Books.Ketab.Select(x => new BookEntity()
                {
                    Catalogs = x.Sureha.Sure.OrderBy(s => TextHelper.ExtractInteger(s.Name)).Select(s =>
                    {
                        var catalogNumber = TextHelper.ExtractInteger(s.Name);
                        pageNumbers++;
                        var result = new CatalogEntity()
                        {
                            Names = new List<ValueEntity>()
                            {
                                new ValueEntity()
                                {
                                    IsMain = isMain,
                                    Language = language,
                                    Value = s.Name
                                }
                            },
                            Number = catalogNumber,
                            Paragraphs = new List<ParagraphEntity>(),
                            StartPageNumber = pageNumbers
                        };
                        foreach (var item in s.Ayat.SelectMany(a => a.Ayat).OrderBy(a => a.Number))
                        {
                            var p = TextHelper.GetParagraph(item.Ayeha.LanguageText.TeXt, language, result, isMain);
                            p.Number = item.Number;
                            result.Paragraphs.Add(p);
                        }
                        result.Pages = new List<PageEntity>()
                        {
                            new PageEntity()
                            {
                                 Paragraphs = result.Paragraphs,
                                 Number = pageNumbers
                            }
                        };
                        return result;
                    }).ToList(),
                    Names = new List<ValueEntity>()
                    {
                        new ValueEntity()
                        {
                            IsMain = isMain,
                            Language = language,
                            Value = x.Name
                        }
                    },
                }).ToList()
            };
        }
    }

    public class BibleCloadRoot
    {
        public BibleCloadMainBook MainBook { get; set; }
    }

    public class BibleCloadSure
    {
        public string Name { get; set; }
        public List<BibleCloadAyat> Ayat { get; set; }
    }

    public class BibleCloadSureha
    {
        public List<BibleCloadSure> Sure { get; set; }
    }
}
