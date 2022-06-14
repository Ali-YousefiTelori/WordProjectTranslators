using Newtonsoft.Json;
using Translators.Contracts.Common;
using Translators.Database.Contexts;
using Translators.Database.Entities;
using Translators.Logics;
using Translators.Models;

namespace Translators.Patches
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                await ConfigData.LoadAsync();
                List<CategoryEntity> books = new List<CategoryEntity>();
                books.Add(await LoadQuran());
                books.Add(await LoadOldTestament());
                books.Add(await LoadNewTestament());
                LogicBase<TranslatorContext, CategoryContract, CategoryEntity> logic = new LogicBase<TranslatorContext, CategoryContract, CategoryEntity>();
                var result = await logic.AddRange(books);
                Console.WriteLine("Started.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.ReadLine();
        }

        static async Task<CategoryEntity> LoadQuran()
        {
            var json = await File.ReadAllTextAsync(@"D:\Github\WordProjectTranslators\src\Resources\quran-uthmani.json");
            var data = JsonConvert.DeserializeObject<AlquranCloadRoot>(json);
            var bookLanguage = new LanguageEntity() { Code = "ar-sa" };
            var books = data.GetBook(bookLanguage);
            //var allText = books.Books.SelectMany(x => x.Catalogs.SelectMany(c => c.Pages.SelectMany(p => p.Paragraphs.SelectMany(g => g.Words.Where(w => w.Value.IsMain).Select(w =>
            //{
            //    string myResult = w.Value.Value + "_" + g.Words.Where(w2 => !w2.Value.IsMain && w2.Index == w.Index).First().Value.Value + "                                                 " + c.Name.Value + c.Number + "_" + p.Number + "_" + g.Number;
            //    return myResult;
            //}))))).ToList();
            //StringBuilder stringBuilder = new StringBuilder();
            //foreach (var item in allText)
            //{
            //    stringBuilder.AppendLine(item);
            //}
            await AddTranslator(books);
            return books;
        }

        static async Task<CategoryEntity> LoadNewTestament()
        {
            var jsonHebrew = await File.ReadAllTextAsync(@"D:\Github\WordProjectTranslators\src\Resources\New Testament Hebrew.json");
            var jsonPersian = await File.ReadAllTextAsync(@"D:\Github\WordProjectTranslators\src\Resources\New Testament Persian.json");
            var dataHebrew = JsonConvert.DeserializeObject<BibleCloadRoot>(jsonHebrew);
            var dataPersian = JsonConvert.DeserializeObject<BibleCloadRoot>(jsonPersian);
            var bookHebrew = dataHebrew.MainBook.GetBook(HebrewLanguage, true);
            var bookPersian = dataPersian.MainBook.GetBook(PersianLanguage, false);
            var catalog =  Merge(bookHebrew, bookPersian);
            foreach (var item in catalog.Books.Skip(4))
            {
                item.IsHidden = true;
            }
            return catalog;
        }

        static CategoryEntity Merge(CategoryEntity category1, CategoryEntity category2)
        {
            category1.Names.AddRange(category2.Names);
            for (int i = 0; i < category1.Books.Count; i++)
            {
                var book = category1.Books[i];
                var book2 = category2.Books[i];
                book.Names.AddRange(book2.Names);
                for (int j = 0; j < book.Catalogs.Count; j++)
                {
                    var catalog = book.Catalogs[j];
                    var catalog2 = book2.Catalogs[j];
                    catalog.Names.AddRange(catalog2.Names);
                    for (int q = 0; q < catalog.Paragraphs.Count; q++)
                    {
                        var paragraphs = catalog.Paragraphs[q];
                        var paragraphs2 = catalog2.Paragraphs[q];
                        paragraphs.Words.AddRange(paragraphs2.Words);
                    }
                }
            }
            return category1;
        }

        static async Task<CategoryEntity> LoadOldTestament()
        {
            var jsonHebrew = await File.ReadAllTextAsync(@"D:\Github\WordProjectTranslators\src\Resources\Old Testament Hebrew.json");
            var jsonPersian = await File.ReadAllTextAsync(@"D:\Github\WordProjectTranslators\src\Resources\Old Testament Persian.json");
            var dataHebrew = JsonConvert.DeserializeObject<BibleCloadRoot>(jsonHebrew);
            var dataPersian = JsonConvert.DeserializeObject<BibleCloadRoot>(jsonPersian);
            var bookHebrew = dataHebrew.MainBook.GetBook(HebrewLanguage, true);
            var bookPersian = dataPersian.MainBook.GetBook(PersianLanguage, false);
            return Merge(bookHebrew, bookPersian);
        }

        static LanguageEntity PersianLanguage = new LanguageEntity()
        {
            Code = "fa-ir",
            Name = "فارسی"
        }; 

        static LanguageEntity HebrewLanguage = new LanguageEntity()
        {
            Code = "heb-he",
            Name = "عبری"
        };

        static TranslatorEntity MakaremTranslator = new TranslatorEntity()
        {
            Names = new List<ValueEntity>()
            {
                 new ValueEntity()
                 {
                      Language = PersianLanguage,
                      Value = "مکارم شیرازی"
                 }
            }
        };

        static async Task AddTranslator(CategoryEntity category)
        {
            var translatequranJson = await File.ReadAllTextAsync(@"D:\Github\WordProjectTranslators\src\Resources\translate makarem quran.json");
            var translateData = JsonConvert.DeserializeObject<TranslatorQuranRoot>(translatequranJson);
            var catalogs = category.Books.First().Catalogs;
            var translatorCatalogs = translateData.MainBook.Books.Ketab.Sureha.Sure;
            for (int i = 0; i < catalogs.Count; i++)
            {
                var catalog = catalogs[i];
                var translatorCatalog = translatorCatalogs[i];
                foreach (var paragraph in catalog.Pages.SelectMany(x => x.Paragraphs).OrderBy(x => x.Number))
                {
                    var ayat = translatorCatalog.Ayat[0].Ayat[(int)paragraph.Number - 1];
                    int index = 1;
                    paragraph.Words.AddRange(ayat.Ayeha.LanguageText.TeXt.Replace("‌", " ").Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s =>
                    {
                        var word = new WordEntity()
                        {
                            Index = index,
                            Values = new List<ValueEntity>()
                            {
                                 new ValueEntity()
                                 {
                                      IsMain = false,
                                      Language = PersianLanguage,
                                      Translator = MakaremTranslator,
                                      Value = s
                                 }
                            }
                        };
                        index++;
                        return word;
                    }));
                }
            }
        }
    }
}