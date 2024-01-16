using EasyMicroservices.TranslatorsMicroservice.Database.Entities;
using EasyMicroservices.Cores.Relational.EntityFrameworkCore;
using EasyMicroservices.Cores.Relational.EntityFrameworkCore.Intrerfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace EasyMicroservices.TranslatorsMicroservice.Database.Contexts
{
    public class ContentContext : RelationalCoreContext
    {
        public ContentContext(IEntityFrameworkCoreDatabaseBuilder builder) : base(builder)
        {
        }

        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<ContentEntity> Translators { get; set; }
        public DbSet<LanguageEntity> Languages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CategoryEntity>(model =>
            {
                model.HasKey(x => x.Id);
                model.Property(x => x.Key).UseCollation("SQL_Latin1_General_CP1_CS_AS");
            });

            modelBuilder.Entity<ContentEntity>(model =>
            {
                model.HasKey(x => x.Id);

                model.HasOne(x => x.Category)
                .WithMany(x => x.Translators)
                .HasForeignKey(x => x.CategoryId);

                model.HasOne(x => x.Language)
                .WithMany(x => x.Translators)
                .HasForeignKey(x => x.LanguageId);
            });

            modelBuilder.Entity<LanguageEntity>(model =>
            {
                model.HasData(
                    new LanguageEntity()
                    {
                        Id = 1,
                        Name = "fa-IR",
                        CreationDateTime = DateTime.Now
                    },
                    new LanguageEntity()
                    {
                        Id = 2,
                        Name = "en-US",
                        CreationDateTime = DateTime.Now
                    }
                );

                model.HasKey(x => x.Id);
                model.HasIndex(x => x.Name).IsUnique();
            });
        }
    }
}