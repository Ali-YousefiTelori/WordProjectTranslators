using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Database.Contexts;

namespace Translators.Logics
{
    public class CacheLogic : IHostedService
    {
        IServiceProvider _serviceProvider;
        public CacheLogic(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<CategoryContract> GetCategory(long id)
        {
            using var context = _serviceProvider.GetService<TranslatorContext>();
            var categoryEntity = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            var category = categoryEntity.Map<CategoryContract>();
            var names = await context.Values
                                .Include(x => x.Translator)
                                .Where(x => x.CategoryNameId == category.Id)
                                .ToListAsync();
            category.Names = names.Select(x => x.Map<ValueContract>()).ToList();
            //category.Books = Books.Select(x => x.Value).Where(x => x.CategoryId == category.Key).ToList();
            return category;
        }

        public async Task<CategoryContract> GetCategoryByBookId(long bookId)
        {
            using var context = _serviceProvider.GetService<TranslatorContext>();
            var bookEntity = await context.Books
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == bookId);
            var category = bookEntity.Category.Map<CategoryContract>();
            var names = await context.Values
                                .Include(x => x.Translator)
                                .Where(x => x.CategoryNameId == category.Id)
                                .ToListAsync();
            category.Names = names.Select(x => x.Map<ValueContract>()).ToList();
            //category.Books = Books.Select(x => x.Value).Where(x => x.CategoryId == category.Key).ToList();
            return category;
        }

        public async Task<BookContract> GetBook(long id)
        {
            using var context = _serviceProvider.GetService<TranslatorContext>();
            var bookEntity = await context.Books.FirstOrDefaultAsync(x => x.Id == id);
            var book = bookEntity.Map<BookContract>();
            var names = await context.Values
                                .Include(x => x.Translator)
                                .Include(x => x.TranslatorName)
                                .Where(x => x.BookNameId == book.Id)
                                .ToListAsync();
            book.Names = names.Select(x => x.Map<ValueContract>()).ToList();
            //    book.Value.Catalogs = Catalogs.Select(x => x.Value).Where(x => x.BookId == book.Key).ToList();
            return book;
        }

        public async Task<CatalogContract> GetCatalog(long id)
        {
            using var context = _serviceProvider.GetService<TranslatorContext>();
            var catalogEntity = await context.Catalogs
                .FirstOrDefaultAsync(x => x.Id == id);
            var catalog = catalogEntity.Map<CatalogContract>();
            var names = await context.Values
                                .Where(x => x.CatalogNameId == catalog.Id).ToListAsync();
            catalog.Names = names.Select(x => x.Map<ValueContract>()).ToList();
            catalog.BookNames = (await GetBook(catalog.BookId)).Names;
            //    catalog.Value.Pages = Pages.Select(x => x.Value).Where(x => x.CatalogId == catalog.Key).ToList();
            return catalog;
        }

        public async Task<long[]> GetCatalogsByPageNumber(long bookId, long pageNumber)
        {
            using var context = _serviceProvider.GetService<TranslatorContext>();
            var pages = await context.Pages
                .Include(x => x.Catalog)
                .Where(x => x.Number == pageNumber && x.Catalog.BookId == bookId)
                .ToListAsync();
            return pages.Select(x => x.CatalogId).Distinct().ToArray();
        }

        public async Task<List<CatalogContract>> GetCatalogsByCatalogIds(long[] catalogIds)
        {
            using var context = _serviceProvider.GetService<TranslatorContext>();
            var bookEntity = await context.Catalogs.Where(x => catalogIds.Contains(x.Id)).ToListAsync();
            var catalogs = bookEntity.Select(x => x.Map<CatalogContract>()).ToList();
            foreach (var catalog in catalogs)
            {
                var names = await context.Values
                                .Include(x => x.Translator)
                                .Include(x => x.TranslatorName)
                                .Where(x => x.CatalogNameId == catalog.Id)
                                .ToListAsync();
                catalog.Names = names.Select(x => x.Map<ValueContract>()).ToList();
                catalog.BookNames = (await GetBook(catalog.BookId)).Names;
            }

            //    catalog.Value.Pages = Pages.Select(x => x.Value).Where(x => x.CatalogId == catalog.Key).ToList();
            return catalogs;
        }

        public async Task<PageContract> GetPage(long id)
        {
            using var context = _serviceProvider.GetService<TranslatorContext>();
            var pageEntity = await context.Pages.FirstOrDefaultAsync(x => x.Id == id);
            var page = pageEntity.Map<PageContract>();
            return page;
        }

        public async Task<List<PageContract>> GetPages(long pageNumber, long[] catalogs)
        {
            using var context = _serviceProvider.GetService<TranslatorContext>();
            var pageEntity = await context.Pages
                .Include(x => x.Paragraphs)
                .ThenInclude(x => x.Words)
                .ThenInclude(x => x.Values)
                .ThenInclude(x => x.Language)
                .Include(x => x.Paragraphs)
                .ThenInclude(x => x.Audios)
                .Include(x => x.Audios)
                .Where(x => x.Number == pageNumber && catalogs.Contains(x.CatalogId))
                .ToListAsync();
            var pages = pageEntity.Select(x => x.Map<PageContract>()).ToList();
            foreach (var page in pages)
            {
                var catalogId = page.Paragraphs.Select(x => x.CatalogId).FirstOrDefault();
                var catalog = await GetCatalog(catalogId);
                page.CatalogNames = catalog.Names;
            }

            return pages;
        }

        public async Task<List<PageContract>> GetPages()
        {
            using var context = _serviceProvider.GetService<TranslatorContext>();
            var pageEntity = await context.Pages.ToListAsync();
            var pages = pageEntity.Select(x => x.Map<PageContract>()).ToList();
            return pages;
        }

        public async Task<ParagraphContract> GetParagraph(long id)
        {
            using var context = _serviceProvider.GetService<TranslatorContext>();
            var paragraphEntity = await context.Paragraphs
                .Include(x => x.Words)
                .ThenInclude(x => x.Values)
                .ThenInclude(x => x.Language)
                .Include(x => x.Words)
                .ThenInclude(x => x.Values)
                .ThenInclude(x => x.Translator)
                .Include(x => x.Words)
                .ThenInclude(x => x.Values)
                .ThenInclude(x => x.TranslatorName)
                .FirstOrDefaultAsync(x => x.Id == id);
            var paragraph = paragraphEntity.Map<ParagraphContract>();
            return paragraph;
        }

        public async Task<WordContract> GetWord(long id)
        {
            using var context = _serviceProvider.GetService<TranslatorContext>();
            var wordEntity = await context.Words.FirstOrDefaultAsync(x => x.Id == id);
            var word = wordEntity.Map<WordContract>();
            return word;
        }

        //public ConcurrentDictionary<long, CategoryContract> Categories { get; set; } = new ConcurrentDictionary<long, CategoryContract>();
        //public ConcurrentDictionary<long, BookContract> Books { get; set; } = new ConcurrentDictionary<long, BookContract>();
        //public ConcurrentDictionary<long, CatalogContract> Catalogs { get; set; } = new ConcurrentDictionary<long, CatalogContract>();
        //public ConcurrentDictionary<long, LanguageContract> Languages { get; set; } = new ConcurrentDictionary<long, LanguageContract>();
        //public ConcurrentDictionary<long, PageContract> Pages { get; set; } = new ConcurrentDictionary<long, PageContract>();
        //public ConcurrentDictionary<long, ParagraphContract> Paragraphs { get; set; } = new ConcurrentDictionary<long, ParagraphContract>();
        //public ConcurrentDictionary<long, TranslatorContract> Translators { get; set; } = new ConcurrentDictionary<long, TranslatorContract>();
        //public ConcurrentDictionary<long, ValueContract> Values { get; set; } = new ConcurrentDictionary<long, ValueContract>();
        //public ConcurrentDictionary<long, WordContract> Words { get; set; } = new ConcurrentDictionary<long, WordContract>();

        async Task Initialize()
        {
            //using var context = _serviceProvider.GetService<TranslatorContext>();
            //await context.Database.MigrateAsync();

            //Console.WriteLine("Load Languages...");
            //var languages = await context.Languages.AsNoTracking().ToListAsync();
            //foreach (var item in languages)
            //{
            //    Languages.TryAdd(item.Id, item.Map<LanguageContract>());
            //}

            //Console.WriteLine("Calculate Values...");

            //foreach (var value in Values)
            //{
            //    if (value.Value.TranslatorId.HasValue)
            //    {
            //        value.Value.Translator = Translators[value.Value.TranslatorId.Value];
            //    }

            //    value.Value.Language = Languages[value.Value.LanguageId];
            //}

            //Console.WriteLine("Calculate Translators...");
            //var hasTranslates = Values.Select(x => x.Value).Where(x => x.TranslatorNameId.HasValue).ToList();
            //foreach (var translator in Translators)
            //{
            //    translator.Value.Names = hasTranslates.Where(x => x.TranslatorNameId == translator.Key).ToList();
            //}
        }

        public async Task FixPageBookIds(List<PageContract> pageContracts)
        {
            foreach (var item in pageContracts)
            {
                item.BookId = (await GetCatalog(item.CatalogId)).BookId;
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Initialize();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
