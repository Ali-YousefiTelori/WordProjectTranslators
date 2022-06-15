using Newtonsoft.Json;
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
                //List<CategoryEntity> books = new List<CategoryEntity>();
                var quran = await LoadQuran();
                //await new LogicBase<TranslatorContext, bool, CategoryEntity>().Add(quran);
                var enjil = await LoadNewTestament();
                //await new LogicBase<TranslatorContext, bool, CategoryEntity>().Add(enjil);
                var torat = await LoadOldTestament();
                //await new LogicBase<TranslatorContext, bool, CategoryEntity>().Add(torat);
                await Save(new List<CategoryEntity>()
                {
                     quran,
                     enjil,
                     torat
                });
                Console.WriteLine("Started.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.ReadLine();
        }

        static async Task Save(List<CategoryEntity> categories)
        {
            Console.WriteLine($"Save categories {categories.Count}");
            using TranslatorContext translatorContext = new TranslatorContext();
            var categoryIndex = 0;
            foreach (var category in categories)
            {
                categoryIndex++;
                var books = category.Books;
                category.Books = null;
                translatorContext.Entry(category).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                await translatorContext.SaveChangesAsync();
                foreach (var item in category.Names)
                {
                    item.CategoryNameId = category.Id;
                }
                await SaveValues(category.Names);

                foreach (var book in books)
                {
                    book.CategoryId = category.Id;
                }
                await SaveBooks(books, categoryIndex);
            }
            Console.WriteLine($"Save Ends...");

            await translatorContext.SaveChangesAsync();
        }

        static async Task SaveBooks(List<BookEntity> books, int categoryIndex)
        {
            using TranslatorContext translatorContext = new TranslatorContext();
            Console.WriteLine($"Save books {books.Count}");
            int bookIndex = 0;
            foreach (var book in books)
            {
                bookIndex++;
                var catalogs = book.Catalogs;
                book.Catalogs = null;
                book.Category = null;
                translatorContext.Entry(book).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                await translatorContext.SaveChangesAsync();
                foreach (var item in book.Names)
                {
                    item.BookNameId = book.Id;
                }
                await SaveValues(book.Names);
                foreach (var catalog in catalogs)
                {
                    catalog.BookId = book.Id;
                }
                await SaveCatalog(catalogs, bookIndex, categoryIndex);
            }
        }

        static async Task SaveCatalog(List<CatalogEntity> catalogs, int bookIndex, int categoryIndex)
        {
            using TranslatorContext translatorContext = new TranslatorContext();
            Console.WriteLine($"Save catalogs {catalogs.Count} book {bookIndex} category {categoryIndex}");
            var suraIndex = 0;
            foreach (var catalog in catalogs)
            {
                suraIndex++;
                var paragraphs = catalog.Paragraphs;
                catalog.Paragraphs = null;
                var pages = catalog.Pages;
                catalog.Pages = null;
                translatorContext.Entry(catalog).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                await translatorContext.SaveChangesAsync();
                foreach (var item in catalog.Names)
                {
                    item.CatalogNameId = catalog.Id;
                }
                await SaveValues(catalog.Names);
                if (paragraphs != null)
                {
                    foreach (var paragraph in paragraphs)
                    {
                        paragraph.CatalogId = catalog.Id;
                    }
                }
                foreach (var page in pages)
                {
                    page.CatalogId = catalog.Id;
                }
                await SavePages(pages, paragraphs == null, suraIndex, bookIndex, categoryIndex);
                if (paragraphs != null)
                    await SaveParagraph(paragraphs);
            }
        }

        static async Task SavePages(List<PageEntity> pages, bool saveParagraphs, int suraIndex, int bookIndex, int categoryIndex)
        {
            using TranslatorContext translatorContext = new TranslatorContext();
            Console.WriteLine($"Save pages {pages.Count} sura {suraIndex} book {bookIndex} category {categoryIndex}");
            foreach (var page in pages)
            {
                var paragraphs = page.Paragraphs;
                page.Paragraphs = null;
                translatorContext.Entry(page).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                await translatorContext.SaveChangesAsync();
                foreach (var paragraph in paragraphs)
                {
                    paragraph.PageId = page.Id;
                    paragraph.CatalogId = page.CatalogId;
                }
                if (saveParagraphs)
                    await SaveParagraph(paragraphs);
            }
        }

        static async Task SaveParagraph(List<ParagraphEntity> paragraphs)
        {
            using TranslatorContext translatorContext = new TranslatorContext();
            Console.WriteLine($"Save paragraphs {paragraphs.Count}");
            foreach (var paragraph in paragraphs)
            {
                translatorContext.Entry(paragraph).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                await translatorContext.SaveChangesAsync();
                foreach (var word in paragraph.Words)
                {
                    word.ParagraphId = paragraph.Id;
                }
                await SaveWords(paragraph.Words);
            }
        }

        static async Task SaveWords(List<WordEntity> words)
        {
            using TranslatorContext translatorContext = new TranslatorContext();
            foreach (var word in words)
            {
                translatorContext.Entry(word).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                await translatorContext.SaveChangesAsync();
                foreach (var value in word.Values)
                {
                    value.WordValueId = word.Id;
                }
                await SaveValues(word.Values);
            }

            await translatorContext.SaveChangesAsync();
        }

        static async Task SaveValues(List<ValueEntity> values)
        {
            using TranslatorContext translatorContext = new TranslatorContext();
            foreach (var value in values)
            {
                translatorContext.Entry(value).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                if (value.Language.Id == 0)
                {
                    translatorContext.Entry(value.Language).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                }
                if ((value.Translator?.Id).GetValueOrDefault(-1) == 0)
                {
                    translatorContext.Entry(value.Translator).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    foreach (var name in value.Translator.Names)
                    {
                        if (name.Id == 0)
                        {
                            translatorContext.Entry(name).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                            if (name.Language.Id == 0)
                                translatorContext.Entry(name.Language).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                        }
                    }
                }
                await translatorContext.SaveChangesAsync();
            }

            await translatorContext.SaveChangesAsync();
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
            var catalog = Merge(bookHebrew, bookPersian);
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