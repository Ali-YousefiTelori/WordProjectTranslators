﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Translators.Database.Contexts;

#nullable disable

namespace Translators.Migrations
{
    [DbContext(typeof(TranslatorContext))]
    [Migration("20221030111338_Add_IsTransliteration")]
    partial class Add_IsTransliteration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Translators.Database.Entities.AppVersionEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<byte>("ApplicationType")
                        .HasColumnType("tinyint");

                    b.Property<int>("CleanCacheTempNumber")
                        .HasColumnType("int");

                    b.Property<int>("ForceUpdateNumber")
                        .HasColumnType("int");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("AppVersions");
                });

            modelBuilder.Entity("Translators.Database.Entities.AudioEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<byte[]>("Data")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsMain")
                        .HasColumnType("bit");

                    b.Property<long?>("LanguageId")
                        .HasColumnType("bigint");

                    b.Property<long?>("PageId")
                        .HasColumnType("bigint");

                    b.Property<long?>("TranslatorId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("LanguageId");

                    b.HasIndex("PageId");

                    b.HasIndex("TranslatorId");

                    b.ToTable("Audioes");
                });

            modelBuilder.Entity("Translators.Database.Entities.Authentications.SMSUserEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("ApiSession")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PatternKey")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SMSUsers");
                });

            modelBuilder.Entity("Translators.Database.Entities.Authentications.UserEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid?>("UserSession")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserName")
                        .IsUnique()
                        .HasFilter("[UserName] IS NOT NULL");

                    b.HasIndex("UserSession");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Translators.Database.Entities.Authentications.UserPermissionEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<byte>("PermissionType")
                        .HasColumnType("tinyint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserPermissions");
                });

            modelBuilder.Entity("Translators.Database.Entities.BookEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("CategoryId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsHidden")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("IsHidden");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("Translators.Database.Entities.CatalogEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("BookId")
                        .HasColumnType("bigint");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("StartPageNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("Number");

                    b.HasIndex("StartPageNumber");

                    b.ToTable("Catalogs");
                });

            modelBuilder.Entity("Translators.Database.Entities.CategoryEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Translators.Database.Entities.LanguageEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Code");

                    b.HasIndex("Name");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("Translators.Database.Entities.LinkGroupEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Title");

                    b.ToTable("LinkGroups");
                });

            modelBuilder.Entity("Translators.Database.Entities.LinkParagraphEntity", b =>
                {
                    b.Property<long>("LinkGroupId")
                        .HasColumnType("bigint");

                    b.Property<long>("ParagraphId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("LinkGroupId", "ParagraphId");

                    b.HasIndex("ParagraphId");

                    b.HasIndex("UserId");

                    b.ToTable("LinkParagraphs");
                });

            modelBuilder.Entity("Translators.Database.Entities.LogEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<int>("AppVersion")
                        .HasColumnType("int");

                    b.Property<string>("DeviceDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogTrace")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Session")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("Translators.Database.Entities.PageEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("CatalogId")
                        .HasColumnType("bigint");

                    b.Property<long>("Number")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CatalogId");

                    b.HasIndex("Number");

                    b.ToTable("Pages");
                });

            modelBuilder.Entity("Translators.Database.Entities.ParagraphEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("AnotherValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("CatalogId")
                        .HasColumnType("bigint");

                    b.Property<long>("Number")
                        .HasColumnType("bigint");

                    b.Property<long>("PageId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CatalogId");

                    b.HasIndex("Number");

                    b.HasIndex("PageId");

                    b.ToTable("Paragraphs");
                });

            modelBuilder.Entity("Translators.Database.Entities.TranslatorEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.HasKey("Id");

                    b.ToTable("Translators");
                });

            modelBuilder.Entity("Translators.Database.Entities.ValueEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long?>("BookNameId")
                        .HasColumnType("bigint");

                    b.Property<long?>("CatalogNameId")
                        .HasColumnType("bigint");

                    b.Property<long?>("CategoryNameId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsMain")
                        .HasColumnType("bit");

                    b.Property<bool>("IsTransliteration")
                        .HasColumnType("bit");

                    b.Property<long>("LanguageId")
                        .HasColumnType("bigint");

                    b.Property<string>("SearchValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("TranslatorId")
                        .HasColumnType("bigint");

                    b.Property<long?>("TranslatorNameId")
                        .HasColumnType("bigint");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long?>("WordValueId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("BookNameId");

                    b.HasIndex("CatalogNameId");

                    b.HasIndex("CategoryNameId");

                    b.HasIndex("IsMain");

                    b.HasIndex("LanguageId");

                    b.HasIndex("TranslatorId");

                    b.HasIndex("TranslatorNameId");

                    b.HasIndex("Value");

                    b.HasIndex("WordValueId");

                    b.ToTable("Values");
                });

            modelBuilder.Entity("Translators.Database.Entities.WordEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<long>("ParagraphId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Index");

                    b.HasIndex("ParagraphId");

                    b.ToTable("Words");
                });

            modelBuilder.Entity("Translators.Database.Entities.WordLetterEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<long>("WordId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Value");

                    b.HasIndex("WordId");

                    b.ToTable("WordLetterEntity");
                });

            modelBuilder.Entity("Translators.Database.Entities.WordRootEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("WordId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Value");

                    b.HasIndex("WordId");

                    b.ToTable("WordRootEntity");
                });

            modelBuilder.Entity("Translators.Database.Entities.AudioEntity", b =>
                {
                    b.HasOne("Translators.Database.Entities.LanguageEntity", "Language")
                        .WithMany("Audios")
                        .HasForeignKey("LanguageId");

                    b.HasOne("Translators.Database.Entities.PageEntity", "Page")
                        .WithMany("Audioes")
                        .HasForeignKey("PageId");

                    b.HasOne("Translators.Database.Entities.TranslatorEntity", "Translator")
                        .WithMany("Audios")
                        .HasForeignKey("TranslatorId");

                    b.Navigation("Language");

                    b.Navigation("Page");

                    b.Navigation("Translator");
                });

            modelBuilder.Entity("Translators.Database.Entities.Authentications.UserPermissionEntity", b =>
                {
                    b.HasOne("Translators.Database.Entities.Authentications.UserEntity", "User")
                        .WithMany("UserPermissions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Translators.Database.Entities.BookEntity", b =>
                {
                    b.HasOne("Translators.Database.Entities.CategoryEntity", "Category")
                        .WithMany("Books")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Translators.Database.Entities.CatalogEntity", b =>
                {
                    b.HasOne("Translators.Database.Entities.BookEntity", "Book")
                        .WithMany("Catalogs")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");
                });

            modelBuilder.Entity("Translators.Database.Entities.LinkParagraphEntity", b =>
                {
                    b.HasOne("Translators.Database.Entities.LinkGroupEntity", "LinkGroup")
                        .WithMany("LinkParagraphs")
                        .HasForeignKey("LinkGroupId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Translators.Database.Entities.ParagraphEntity", "Paragraph")
                        .WithMany("LinkParagraphs")
                        .HasForeignKey("ParagraphId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Translators.Database.Entities.Authentications.UserEntity", "User")
                        .WithMany("LinkParagraphs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("LinkGroup");

                    b.Navigation("Paragraph");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Translators.Database.Entities.PageEntity", b =>
                {
                    b.HasOne("Translators.Database.Entities.CatalogEntity", "Catalog")
                        .WithMany("Pages")
                        .HasForeignKey("CatalogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Catalog");
                });

            modelBuilder.Entity("Translators.Database.Entities.ParagraphEntity", b =>
                {
                    b.HasOne("Translators.Database.Entities.CatalogEntity", "Catalog")
                        .WithMany("Paragraphs")
                        .HasForeignKey("CatalogId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Translators.Database.Entities.PageEntity", "Page")
                        .WithMany("Paragraphs")
                        .HasForeignKey("PageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Catalog");

                    b.Navigation("Page");
                });

            modelBuilder.Entity("Translators.Database.Entities.ValueEntity", b =>
                {
                    b.HasOne("Translators.Database.Entities.BookEntity", "BookName")
                        .WithMany("Names")
                        .HasForeignKey("BookNameId");

                    b.HasOne("Translators.Database.Entities.CatalogEntity", "Catalog")
                        .WithMany("Names")
                        .HasForeignKey("CatalogNameId");

                    b.HasOne("Translators.Database.Entities.CategoryEntity", "Category")
                        .WithMany("Names")
                        .HasForeignKey("CategoryNameId");

                    b.HasOne("Translators.Database.Entities.LanguageEntity", "Language")
                        .WithMany("Values")
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Translators.Database.Entities.TranslatorEntity", "Translator")
                        .WithMany("Values")
                        .HasForeignKey("TranslatorId");

                    b.HasOne("Translators.Database.Entities.TranslatorEntity", "TranslatorName")
                        .WithMany("Names")
                        .HasForeignKey("TranslatorNameId");

                    b.HasOne("Translators.Database.Entities.WordEntity", "Word")
                        .WithMany("Values")
                        .HasForeignKey("WordValueId");

                    b.Navigation("BookName");

                    b.Navigation("Catalog");

                    b.Navigation("Category");

                    b.Navigation("Language");

                    b.Navigation("Translator");

                    b.Navigation("TranslatorName");

                    b.Navigation("Word");
                });

            modelBuilder.Entity("Translators.Database.Entities.WordEntity", b =>
                {
                    b.HasOne("Translators.Database.Entities.ParagraphEntity", "Paragraph")
                        .WithMany("Words")
                        .HasForeignKey("ParagraphId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Paragraph");
                });

            modelBuilder.Entity("Translators.Database.Entities.WordLetterEntity", b =>
                {
                    b.HasOne("Translators.Database.Entities.WordEntity", "Word")
                        .WithMany("WordLetters")
                        .HasForeignKey("WordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Word");
                });

            modelBuilder.Entity("Translators.Database.Entities.WordRootEntity", b =>
                {
                    b.HasOne("Translators.Database.Entities.WordEntity", "Word")
                        .WithMany("WordRoots")
                        .HasForeignKey("WordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Word");
                });

            modelBuilder.Entity("Translators.Database.Entities.Authentications.UserEntity", b =>
                {
                    b.Navigation("LinkParagraphs");

                    b.Navigation("UserPermissions");
                });

            modelBuilder.Entity("Translators.Database.Entities.BookEntity", b =>
                {
                    b.Navigation("Catalogs");

                    b.Navigation("Names");
                });

            modelBuilder.Entity("Translators.Database.Entities.CatalogEntity", b =>
                {
                    b.Navigation("Names");

                    b.Navigation("Pages");

                    b.Navigation("Paragraphs");
                });

            modelBuilder.Entity("Translators.Database.Entities.CategoryEntity", b =>
                {
                    b.Navigation("Books");

                    b.Navigation("Names");
                });

            modelBuilder.Entity("Translators.Database.Entities.LanguageEntity", b =>
                {
                    b.Navigation("Audios");

                    b.Navigation("Values");
                });

            modelBuilder.Entity("Translators.Database.Entities.LinkGroupEntity", b =>
                {
                    b.Navigation("LinkParagraphs");
                });

            modelBuilder.Entity("Translators.Database.Entities.PageEntity", b =>
                {
                    b.Navigation("Audioes");

                    b.Navigation("Paragraphs");
                });

            modelBuilder.Entity("Translators.Database.Entities.ParagraphEntity", b =>
                {
                    b.Navigation("LinkParagraphs");

                    b.Navigation("Words");
                });

            modelBuilder.Entity("Translators.Database.Entities.TranslatorEntity", b =>
                {
                    b.Navigation("Audios");

                    b.Navigation("Names");

                    b.Navigation("Values");
                });

            modelBuilder.Entity("Translators.Database.Entities.WordEntity", b =>
                {
                    b.Navigation("Values");

                    b.Navigation("WordLetters");

                    b.Navigation("WordRoots");
                });
#pragma warning restore 612, 618
        }
    }
}
