using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Translators.Contracts.Common;
using Translators.Database.Contexts;
using Translators.Schemas;
using Translators.Schemas.Bases;
using Translators.Shared.FileVersionControl;

namespace Translators.Logics
{
    public static class CacheLogic
    {
        public static ConcurrentDictionary<long, CategoryContract> Categories { get; set; } = new ConcurrentDictionary<long, CategoryContract>();
        public static ConcurrentDictionary<long, BookContract> Books { get; set; } = new ConcurrentDictionary<long, BookContract>();
        public static ConcurrentDictionary<long, CatalogContract> Catalogs { get; set; } = new ConcurrentDictionary<long, CatalogContract>();
        public static ConcurrentDictionary<long, LanguageContract> Languages { get; set; } = new ConcurrentDictionary<long, LanguageContract>();
        public static ConcurrentDictionary<long, PageContract> Pages { get; set; } = new ConcurrentDictionary<long, PageContract>();
        public static ConcurrentDictionary<long, ParagraphContract> Paragraphs { get; set; } = new ConcurrentDictionary<long, ParagraphContract>();
        public static ConcurrentDictionary<long, TranslatorContract> Translators { get; set; } = new ConcurrentDictionary<long, TranslatorContract>();
        public static ConcurrentDictionary<long, ValueContract> Values { get; set; } = new ConcurrentDictionary<long, ValueContract>();
        public static ConcurrentDictionary<long, WordContract> Words { get; set; } = new ConcurrentDictionary<long, WordContract>();

        public static async Task Initialize()
        {
            await using var context = new TranslatorContext();
            await context.Database.MigrateAsync();
            Console.WriteLine("Load Categories...");
            var categories = await context.Categories.AsNoTracking().ToListAsync();
            foreach (var item in categories)
            {
                Categories.TryAdd(item.Id, item.Map<CategoryContract>());
            }

            Console.WriteLine("Load Books...");
            var books = await context.Books.AsNoTracking().ToListAsync();
            foreach (var item in books)
            {
                Books.TryAdd(item.Id, item.Map<BookContract>());
            }

            Console.WriteLine("Load Catalogs...");
            var catalogs = await context.Catalogs.AsNoTracking().ToListAsync();
            foreach (var item in catalogs)
            {
                Catalogs.TryAdd(item.Id, item.Map<CatalogContract>());
            }

            Console.WriteLine("Load Languages...");
            var languages = await context.Languages.AsNoTracking().ToListAsync();
            foreach (var item in languages)
            {
                Languages.TryAdd(item.Id, item.Map<LanguageContract>());
            }

            Console.WriteLine("Load Pages...");
            var pages = await context.Pages.Include(x => x.Audios).AsNoTracking().ToListAsync();
            foreach (var item in pages)
            {
                Pages.TryAdd(item.Id, item.Map<PageContract>());
            }

            FixPageBookIds(Pages.Values.ToList());

            Console.WriteLine("Load Paragraphs...");
            var paragraphs = await context.Paragraphs.Include(x => x.Audios).Include(x => x.LinkParagraphs).AsNoTracking().ToListAsync();
            foreach (var item in paragraphs)
            {
                Paragraphs.TryAdd(item.Id, item.Map<ParagraphContract>());
            }

            Console.WriteLine("Load Translators...");
            var translators = await context.Translators.AsNoTracking().ToListAsync();
            foreach (var item in translators)
            {
                Translators.TryAdd(item.Id, item.Map<TranslatorContract>());
            }

            await using var context2 = new TranslatorContext();
            Console.WriteLine("Load Values...");
            var values = await context2.Values.AsNoTracking().ToListAsync();
            foreach (var item in values)
            {
                Values.TryAdd(item.Id, item.Map<ValueContract>());
            }

            var words = await context2.Words.ToListAsync();
            Console.WriteLine("Load Words...");
            foreach (var item in words)
            {
                Words.TryAdd(item.Id, item.Map<WordContract>());
            }

            Console.WriteLine("Calculate Values...");

            foreach (var value in Values)
            {
                if (value.Value.TranslatorId.HasValue)
                {
                    value.Value.Translator = Translators[value.Value.TranslatorId.Value];
                }

                value.Value.Language = Languages[value.Value.LanguageId];
            }

            Console.WriteLine("Calculate Translators...");
            var hasTranslates = Values.Select(x => x.Value).Where(x => x.TranslatorNameId.HasValue).ToList();
            foreach (var translator in Translators)
            {
                translator.Value.Names = hasTranslates.Where(x => x.TranslatorNameId == translator.Key).ToList();
            }

            Console.WriteLine("Calculate Words...");
            var hasWords = Values.Select(x => x.Value).Where(x => x.WordValueId.HasValue).GroupBy(x => x.WordValueId).ToDictionary(x => x.Key, x => x.ToList());

            foreach (var word in Words)
            {
                word.Value.Values = hasWords[word.Key];
            }

            Console.WriteLine("Calculate Paragraphs...");
            var hasParagraph = Words.Select(x => x.Value).GroupBy(x => x.ParagraphId).ToDictionary(x => x.Key, x => x.ToList());
            foreach (var paragraph in Paragraphs)
            {
                paragraph.Value.Words = hasParagraph[paragraph.Key];
            }

            Console.WriteLine("Calculate Pages...");
            var hasCatalogs = Values.Select(x => x.Value).Where(x => x.CatalogNameId.HasValue).ToList();
            foreach (var page in Pages)
            {
                page.Value.Paragraphs = Paragraphs.Select(x => x.Value).Where(x => x.PageId == page.Key).ToList();
                page.Value.CatalogNames = page.Value.Paragraphs.SelectMany(p => hasCatalogs.Where(x => x.CatalogNameId == p.CatalogId)).Distinct().ToList();
            }

            Console.WriteLine("Calculate Catalogs...");
            foreach (var catalog in Catalogs)
            {
                catalog.Value.Names = hasCatalogs.Where(x => x.CatalogNameId == catalog.Key).ToList();
                catalog.Value.Pages = Pages.Select(x => x.Value).Where(x => x.CatalogId == catalog.Key).ToList();
            }

            Console.WriteLine("Calculate Books...");
            var hasBooks = Values.Select(x => x.Value).Where(x => x.BookNameId.HasValue).ToList();
            foreach (var book in Books)
            {
                book.Value.Names = hasBooks.Where(x => x.BookNameId == book.Key).ToList();
                book.Value.Catalogs = Catalogs.Select(x => x.Value).Where(x => x.BookId == book.Key).ToList();
            }

            Console.WriteLine("Calculate Categories...");
            var hasCategories = Values.Select(x => x.Value).Where(x => x.CategoryNameId.HasValue).ToList();
            foreach (var category in Categories)
            {
                category.Value.Names = hasCategories.Where(x => x.CategoryNameId == category.Key).ToList();
                category.Value.Books = Books.Select(x => x.Value).Where(x => x.CategoryId == category.Key).ToList();
            }

            _ = SaveVersioning();
        }

        static async Task SaveVersioning()
        {
            try
            {
                Console.WriteLine("Saving...");
                var mappedCategories = Categories.Values.Select(x => x.Map<CategorySchemaBase>()).ToList();
                await SchemaVersionControl.Current.SaveAndUpdateSchema(mappedCategories, "Categories");

                var mappedValues = Values.Values.Select(x => x.Map<ValueSchema>()).ToList();
                await SchemaVersionControl.Current.SaveAndUpdateSchema(mappedValues, "Values");

                Console.WriteLine("Saving finished!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void FixPageBookIds(List<PageContract> pageContracts)
        {
            foreach (var item in pageContracts)
            {
                item.BookId = Catalogs[item.CatalogId].BookId;
            }
        }
    }
}
