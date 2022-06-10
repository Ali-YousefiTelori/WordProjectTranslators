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
                LogicBase<TranslatorContext, CategoryContract, CategoryEntity> logic = new LogicBase<TranslatorContext, CategoryContract, CategoryEntity>();
                var result = await logic.Add(books);
                Console.WriteLine("Started.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.ReadLine();
        }

        static LanguageEntity PersianLanguage = new LanguageEntity()
        {
            Code = "fa-ir",
            Name = "فارسی"
        };

        static TranslatorEntity MakaremTranslator = new  TranslatorEntity()
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