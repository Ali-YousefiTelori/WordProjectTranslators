using Newtonsoft.Json;
using System.Text;
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
                LogicBase<TranslatorContext, LanguageContract, LanguageEntity> logicLanguage = new LogicBase<TranslatorContext, LanguageContract, LanguageEntity>();
                var searchLanguage = new LanguageContract() { Code = "search", Name = "Search Language" };
                var searchLanguageResult = await logicLanguage.Add(searchLanguage);
                Ayah.SearchLanguageId = searchLanguageResult.Result.Id;
                var bookLanguage = new Contracts.Common.LanguageContract() { Code = "ar-sa" };
                var bookLanguageResult = await logicLanguage.Add(bookLanguage);
                var books = data.GetBook(bookLanguageResult.Result.Id);
                var allText = books.Books.SelectMany(x => x.Catalogs.SelectMany(c => c.Pages.SelectMany(p => p.Paragraphs.SelectMany(g => g.Words.Where(w => w.Value.IsMain).Select(w =>
                {
                    string myResult = w.Value.Value + "_" + g.Words.Where(w2 => !w2.Value.IsMain && w2.Index == w.Index).First().Value.Value + "                                                 " + c.Name.Value + c.Number + "_" + p.Number + "_" + g.Number;
                    return myResult;
                }))))).ToList();
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var item in allText)
                {
                    stringBuilder.AppendLine(item);
                }

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
    }
}