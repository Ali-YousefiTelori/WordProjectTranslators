using Microsoft.EntityFrameworkCore;
using Translators.Database.Entities;

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
        public DbSet<LanguageValueEntity> LanguageValues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<LanguageValueEntity>(x =>
            {
                x.HasKey(r => r.Id);
                x.HasIndex(x => x.Value);
                x.HasIndex(x => x.IsMain);

                x.HasOne(x => x.Language)
                 .WithMany(x => x.LanguageValues)
                 .HasForeignKey(x => x.LanguageId);
            });

            modelBuilder.Entity<CategoryEntity>(x =>
            {
                x.HasKey(r => r.Id);

                x.HasOne(x => x.Name)
                 .WithMany(x => x.Categories)
                 .HasForeignKey(x => x.NameId);
            });

            modelBuilder.Entity<BookEntity>(x =>
            {
                x.HasKey(r => r.Id);

                x.HasOne(x => x.Name)
                 .WithMany(x => x.Books)
                 .HasForeignKey(x => x.NameId);

                x.HasOne(x => x.Category)
                 .WithMany(x => x.Books)
                 .HasForeignKey(x => x.CategoryId);
            });

            modelBuilder.Entity<CatalogEntity>(x =>
            {
                x.HasKey(r => r.Id);

                x.HasOne(x => x.Name)
                 .WithMany(x => x.Catalogs)
                 .HasForeignKey(x => x.NameId);

                x.HasOne(x => x.Book)
                 .WithMany(x => x.Catalogs)
                 .HasForeignKey(x => x.BookId);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
