using Microsoft.EntityFrameworkCore;
using Translators.Database.Entities;
using Translators.Models;

namespace Translators.Database.Contexts
{
    public class TranslatorContext : DbContext
    {
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<BookEntity> Books { get; set; }
        public DbSet<CatalogEntity> Catalogs { get; set; }
        public DbSet<PageEntity> Pages { get; set; }
        public DbSet<ParagraphEntity> Paragraphs { get; set; }
        public DbSet<WordEntity> Words { get; set; }
        public DbSet<LanguageEntity> Languages { get; set; }
        public DbSet<ValueEntity> Values { get; set; }
        public DbSet<TranslatorEntity> Translators { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LanguageEntity>(x =>
            {
                x.HasKey(r => r.Id);
                x.HasIndex(x => x.Name);
                x.HasIndex(x => x.Code);
            });

            modelBuilder.Entity<ValueEntity>(x =>
            {
                x.HasKey(r => r.Id);
                x.HasIndex(x => x.Value);
                x.HasIndex(x => x.IsMain);

                x.HasOne(x => x.Language)
                 .WithMany(x => x.Values)
                 .HasForeignKey(x => x.LanguageId);

                x.HasOne(x => x.Translator)
                 .WithMany(x => x.Values)
                 .HasForeignKey(x => x.TranslatorId);

                x.HasOne(x => x.TranslatorName)
                 .WithMany(x => x.Names)
                 .HasForeignKey(x => x.TranslatorNameId);

                x.HasOne(x => x.BookName)
                 .WithMany(x => x.Names)
                 .HasForeignKey(x => x.BookNameId);

                x.HasOne(x => x.Category)
                 .WithMany(x => x.Names)
                 .HasForeignKey(x => x.CategoryNameId);

                x.HasOne(x => x.Catalog)
                 .WithMany(x => x.Names)
                 .HasForeignKey(x => x.CatalogNameId);

                x.HasOne(x => x.Word)
                 .WithMany(x => x.Values)
                 .HasForeignKey(x => x.WordValueId);
            });

            modelBuilder.Entity<CategoryEntity>(x =>
            {
                x.HasKey(r => r.Id);
            });

            modelBuilder.Entity<BookEntity>(x =>
            {
                x.HasKey(r => r.Id);

                x.HasOne(x => x.Category)
                 .WithMany(x => x.Books)
                 .HasForeignKey(x => x.CategoryId);
            });

            modelBuilder.Entity<CatalogEntity>(x =>
            {
                x.HasKey(r => r.Id);

                x.HasOne(x => x.Book)
                 .WithMany(x => x.Catalogs)
                 .HasForeignKey(x => x.BookId);
            });

            modelBuilder.Entity<PageEntity>(x =>
            {
                x.HasKey(r => r.Id);
                x.HasIndex(r => r.Number);

                x.HasOne(x => x.Catalog)
                 .WithMany(x => x.Pages)
                 .HasForeignKey(x => x.CatalogId);
            });

            modelBuilder.Entity<ParagraphEntity>(x =>
            {
                x.HasKey(r => r.Id);
                x.HasIndex(r => r.Number);

                x.HasOne(x => x.Page)
                 .WithMany(x => x.Paragraphs)
                 .HasForeignKey(x => x.PageId);

                x.HasOne(x => x.Catalog)
                 .WithMany(x => x.Paragraphs)
                 .HasForeignKey(x => x.CatalogId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<WordEntity>(x =>
            {
                x.HasKey(r => r.Id);
                x.HasIndex(r => r.Index);

                x.HasOne(x => x.Paragraph)
                 .WithMany(x => x.Words)
                 .HasForeignKey(x => x.ParagraphId);
            });

            modelBuilder.Entity<WordLetterEntity>(x =>
            {
                x.HasKey(r => r.Id);
                x.HasIndex(r => r.Value);

                x.HasOne(x => x.Word)
                 .WithMany(x => x.WordLetters)
                 .HasForeignKey(x => x.WordId);
            });

            modelBuilder.Entity<WordRootEntity>(x =>
            {
                x.HasKey(r => r.Id);
                x.HasIndex(r => r.Value);

                x.HasOne(x => x.Word)
                 .WithMany(x => x.WordRoots)
                 .HasForeignKey(x => x.WordId);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigData.Load();
            optionsBuilder.UseSqlServer(ConfigData.Current.ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
