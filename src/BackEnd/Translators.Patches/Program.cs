using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Translators.Database.Contexts;
using Translators.Database.Entities;
using Translators.Helpers;
using Translators.Models;

namespace Translators.Patches
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting...");
                await ConfigData.LoadAsync();
                await UpdateEnjilToAramicValues();
                Console.WriteLine("Started!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.ReadLine();
        }


        static async Task UpdateEnjilToAramicValues()
        {
            var lines = await File.ReadAllLinesAsync(@"D:\Github\WordProjectTranslators\src\Resources\Holy-Bible---Aramaic---Aramaic-NT-Peshitta---Source-Edition.UNBOUND.txt");
            using TranslatorContext translatorContext = new TranslatorContext();
            //var languages = await translatorContext.Languages.ToListAsync();
            var books = await translatorContext.Books.Where(x => x.CategoryId == 2).ToListAsync();
            var catalogs = await translatorContext.Catalogs.ToListAsync();
            var paragraphs = await translatorContext.Paragraphs.ToListAsync();
            var words = await translatorContext.Words.ToListAsync();
            var values = await translatorContext.Values.ToListAsync();
            int count = lines.Count() - 1;
            int index = 0;
            foreach (var line in lines.Skip(1))
            {
                var split = line.Split('\t');
                var bookNumber = new string(split[0].Where(x => char.IsDigit(x)).ToArray());
                var SurahNumber = split[1];
                var verseNumber = split[2];
                var text = split[3].Replace("܀", "").Trim();

                var book = books.FirstOrDefault(x => (x.Id - 2) == int.Parse(bookNumber) - 40);
                var catalog = book.Catalogs.FirstOrDefault(x => x.StartPageNumber.ToString() == SurahNumber);
                var verse = catalog.Paragraphs.FirstOrDefault(x => x.Number.ToString() == verseNumber);
                var removedWords = verse.Words.Where(x => x.Values.Any(w => w.IsMain)).ToList();
                foreach (var item in removedWords)
                {
                    verse.Words.Remove(item);
                }
                translatorContext.Words.RemoveRange(removedWords);
                var createdWords = text.Replace("  ", "").Split(" ").ToArray();

                for (int i = 0; i < createdWords.Length; i++)
                {
                    var word = new WordEntity()
                    {
                        Index = i,
                        Values = new List<ValueEntity>()
                        {
                            new ValueEntity()
                            {
                                IsMain = true,
                                LanguageId = 4,
                                SearchValue = createdWords[i],
                                Value = createdWords[i]
                            }
                        },
                        ParagraphId = verse.Id,
                    };
                    verse.Words.Add(word);
                    translatorContext.Words.Add(word);
                }
                index++;
                await translatorContext.SaveChangesAsync();
                Console.WriteLine($"Comepleted {index}/{count}");
            }

        }

        static async Task UploadVoices()
        {
            await VoidUploader.Upload();
        }

        static async Task InitializeBooks()
        {
            var translatedBible = await LoadBibleCloadRoot(@"D:\Github\WordProjectTranslators\src\Resources\fa_new\index.htm", PersianLanguage, false);
            var mainBible = await LoadBibleCloadRoot(@"D:\Github\WordProjectTranslators\src\Resources\he_new\index.htm", HebrewLanguage, true);
            //List<CategoryEntity> books = new List<CategoryEntity>();
            var quran = await LoadQuran();
            //await new LogicBase<TranslatorContext, bool, CategoryEntity>().Add(quran);
            var torat = Merge(mainBible.Old, translatedBible.Old);//await LoadNewTestament();
                                                                  //await new LogicBase<TranslatorContext, bool, CategoryEntity>().Add(enjil);
            var enjil = Merge(mainBible.New, translatedBible.New);// await LoadOldTestament();
                                                                  //await new LogicBase<TranslatorContext, bool, CategoryEntity>().Add(torat);
            await Save(new List<CategoryEntity>()
            {
                 quran,
                 enjil,
                 torat
            });
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
                        try
                        {
                            var paragraphs = catalog.Paragraphs[q];
                            var paragraphs2 = catalog2.Paragraphs[q];
                            paragraphs.Words.AddRange(paragraphs2.Words);
                        }
                        catch (Exception ex)
                        {

                        }
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
                    paragraph.Words.AddRange(ayat.Ayeha.LanguageText.TeXt.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s =>
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
                                      Value = s,
                                      SearchValue = TextHelper.FixArabicForSearch(new string(TextHelper.Clean(s).Where(x => TextHelper.SearchChars.Contains(x)).ToArray()))
                                 }
                            }
                        };
                        index++;
                        return word;
                    }));
                }
            }
        }

        static async Task<(CategoryEntity Old, CategoryEntity New)> LoadBibleCloadRoot(string fileName, LanguageEntity language, bool isMain)
        {
            var text = await File.ReadAllTextAsync(fileName, System.Text.Encoding.UTF8);
            var splitBooks = text.Split("h4center");
            var oldBible = ExtractBooks(splitBooks[1]);
            var oldBilbleChapters = await ExtractChapters(fileName, oldBible);

            var bookOld = GetBook("عهد عتیق", oldBilbleChapters, language, isMain);

            var newBible = ExtractBooks(splitBooks[2]);
            var newBibleChapters = await ExtractChapters(fileName, newBible);
            var bookNew = GetBook("عهد جدید", newBibleChapters, language, isMain);
            foreach (var item in bookNew.Books.Skip(4))
            {
                item.IsHidden = true;
            }
            return (bookOld, bookNew);
        }

        static List<(string Name, string Path)> ExtractBooks(string data)
        {
            List<(string Name, string Path)> result = new List<(string Name, string Path)>();
            Regex regex = new Regex(@"<li>(.*?)</li>");
            var matchs = regex.Matches(data);
            foreach (var item in matchs)
            {
                var path = Regex.Match(item.ToString(), @"href=""(.*?)"">").Groups[1].Value;
                var name = Regex.Match(item.ToString(), @"<bdo dir=""rtl"">(.*?)<").Groups[1].Value;
                result.Add((name.Trim(), path.Trim()));
            }

            return result;
        }

        static async Task<Dictionary<string, Dictionary<(string Name, int Number), List<(int Number, string text)>>>> ExtractChapters(string fileName, List<(string Name, string Path)> books)
        {
            Dictionary<string, Dictionary<(string Name, int Number), List<(int Number, string text)>>> result = new Dictionary<string, Dictionary<(string Name, int Number), List<(int Number, string text)>>>();
            foreach (var book in books)
            {
                Dictionary<(string Name, int Number), List<(int Number, string text)>> chapters = new Dictionary<(string Name, int Number), List<(int Number, string text)>>(0);

                result.Add(book.Name, chapters);
                var dirPath = Path.Combine(Path.GetDirectoryName(fileName), Path.GetDirectoryName(book.Path));

                foreach (var chapterFileName in Directory.GetFiles(dirPath))
                {
                    var chapterNumber = Path.GetFileNameWithoutExtension(chapterFileName);
                    var fileData = await File.ReadAllTextAsync(chapterFileName, System.Text.Encoding.UTF8);
                    var matchs = Regex.Matches(fileData, @"verse.*");
                    var nameMatch = Regex.Match(fileData, @"<bdo dir=""rtl"">(.*?)</h3>").Groups[1].Value.Replace("</bdo>", "").Replace("&nbsp;", "").Replace("  ", " ").Trim();
                    List<(int Number, string text)> verses = new List<(int Number, string text)>();
                    chapters.Add((nameMatch, int.Parse(chapterNumber)), verses);

                    foreach (var item in matchs)
                    {
                        var id = Regex.Match(item.ToString(), @"id=""(.*?)"">").Groups[1].Value;
                        var text = Regex.Match(item.ToString(), @"span>(.*)").Groups[1].Value;
                        if (text.Contains("</p>"))
                            text = text.Split("</p>").FirstOrDefault().Trim();
                        text = TextHelper.RemoveNumbers(text.Trim().Replace("span>", "").Replace("<br />", "").Replace("</a>", "").Replace("&nbsp;", "")).Trim();
                        if ("0123456789abcdefghijklmnopqrstuvwxyz".Any(c => text.Any(x => x.ToString().Equals(c.ToString(), StringComparison.OrdinalIgnoreCase))))
                        {
                            //در نسخه ی fa_new\66 شما نیاز دارید در ایه ی 20 ,16,19,18, 14 یک اینتر بزنید
                            //throw new Exception("Unspected char detected!");
                        }
                        verses.Add((int.Parse(id), text));
                    }
                }
            }

            return result;
        }

        public static CategoryEntity GetBook(string bookName, Dictionary<string, Dictionary<(string Name, int Number), List<(int Number, string text)>>> bookBase, LanguageEntity language, bool isMain)
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
                        Value = bookName
                    }
                },
                Books = bookBase.Select(x => new BookEntity()
                {
                    Catalogs = x.Value.OrderBy(s => s.Key.Number).Select(s =>
                    {
                        var catalogNumber = s.Key.Number;
                        pageNumbers = s.Key.Number;
                        var result = new CatalogEntity()
                        {
                            Names = new List<ValueEntity>()
                            {
                                new ValueEntity()
                                {
                                    IsMain = isMain,
                                    Language = language,
                                    Value = s.Key.Name
                                }
                            },
                            Number = catalogNumber,
                            Paragraphs = new List<ParagraphEntity>(),
                            StartPageNumber = pageNumbers
                        };
                        foreach (var item in s.Value.OrderBy(a => a.Number))
                        {
                            var p = TextHelper.GetParagraph(item.text, language, result, isMain);
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
                            Value = x.Key
                        }
                    },
                }).ToList()
            };
        }
    }
}