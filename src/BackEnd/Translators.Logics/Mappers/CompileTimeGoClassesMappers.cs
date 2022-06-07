using System.Linq;
using System;

namespace CompileTimeGoMapper
{
    public static class BookEntity_BookContractMapper
    {
        public static global::Translators.Database.Entities.BookEntity Map(this global::Translators.Contracts.Common.BookContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Database.Entities.BookEntity()
            {
                Id = toMap.Id,
                NameId = toMap.NameId,
                Name = toMap.Name.Map(uniqueRecordId, language, parameters),
                CategoryId = toMap.CategoryId,
                Catalogs = toMap.Catalogs?.Select(x => x.Map(uniqueRecordId, language, parameters)).ToList(),
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.BookContract Map(this global::Translators.Database.Entities.BookEntity toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.BookContract()
            {
                Id = toMap.Id,
                NameId = toMap.NameId,
                Name = toMap.Name.Map(uniqueRecordId, language, parameters),
                CategoryId = toMap.CategoryId,
                Catalogs = toMap.Catalogs?.Select(x => x.Map(uniqueRecordId, language, parameters)).ToList(),
            };

            return mapped;
        }
    }
    public static class LanguageValueEntity_LanguageValueContractMapper
    {
        public static global::Translators.Database.Entities.LanguageValueEntity Map(this global::Translators.Contracts.Common.LanguageValueContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Database.Entities.LanguageValueEntity()
            {
                Id = toMap.Id,
                IsMain = toMap.IsMain,
                Value = toMap.Value,
                LanguageId = toMap.LanguageId,
                Language = toMap.Language.Map(uniqueRecordId, language, parameters),
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.LanguageValueContract Map(this global::Translators.Database.Entities.LanguageValueEntity toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.LanguageValueContract()
            {
                Id = toMap.Id,
                IsMain = toMap.IsMain,
                Value = toMap.Value,
                LanguageId = toMap.LanguageId,
                Language = toMap.Language.Map(uniqueRecordId, language, parameters),
            };

            return mapped;
        }
    }
    public static class CatalogEntity_CatalogContractMapper
    {
        public static global::Translators.Database.Entities.CatalogEntity Map(this global::Translators.Contracts.Common.CatalogContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Database.Entities.CatalogEntity()
            {
                Id = toMap.Id,
                Number = toMap.Number,
                NameId = toMap.NameId,
                Name = toMap.Name.Map(uniqueRecordId, language, parameters),
                BookId = toMap.BookId,
                Pages = toMap.Pages?.Select(x => x.Map(uniqueRecordId, language, parameters)).ToList(),
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.CatalogContract Map(this global::Translators.Database.Entities.CatalogEntity toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.CatalogContract()
            {
                Id = toMap.Id,
                Number = toMap.Number,
                NameId = toMap.NameId,
                Name = toMap.Name.Map(uniqueRecordId, language, parameters),
                BookId = toMap.BookId,
                Pages = toMap.Pages?.Select(x => x.Map(uniqueRecordId, language, parameters)).ToList(),
            };

            return mapped;
        }
    }
    public static class LanguageEntity_LanguageContractMapper
    {
        public static global::Translators.Database.Entities.LanguageEntity Map(this global::Translators.Contracts.Common.LanguageContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Database.Entities.LanguageEntity()
            {
                Id = toMap.Id,
                Name = toMap.Name,
                Code = toMap.Code,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.LanguageContract Map(this global::Translators.Database.Entities.LanguageEntity toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.LanguageContract()
            {
                Id = toMap.Id,
                Name = toMap.Name,
                Code = toMap.Code,
            };

            return mapped;
        }
    }
    public static class PageEntity_PageContractMapper
    {
        public static global::Translators.Database.Entities.PageEntity Map(this global::Translators.Contracts.Common.PageContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Database.Entities.PageEntity()
            {
                Id = toMap.Id,
                Number = toMap.Number,
                CatalogId = toMap.CatalogId,
                Paragraphs = toMap.Paragraphs?.Select(x => x.Map(uniqueRecordId, language, parameters)).ToList(),
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.PageContract Map(this global::Translators.Database.Entities.PageEntity toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.PageContract()
            {
                Id = toMap.Id,
                Number = toMap.Number,
                CatalogId = toMap.CatalogId,
                Paragraphs = toMap.Paragraphs?.Select(x => x.Map(uniqueRecordId, language, parameters)).ToList(),
            };

            return mapped;
        }
    }
    public static class CategoryEntity_CategoryContractMapper
    {
        public static global::Translators.Database.Entities.CategoryEntity Map(this global::Translators.Contracts.Common.CategoryContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Database.Entities.CategoryEntity()
            {
                Id = toMap.Id,
                NameId = toMap.NameId,
                Name = toMap.Name.Map(uniqueRecordId, language, parameters),
                Books = toMap.Books?.Select(x => x.Map(uniqueRecordId, language, parameters)).ToList(),
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.CategoryContract Map(this global::Translators.Database.Entities.CategoryEntity toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.CategoryContract()
            {
                Id = toMap.Id,
                NameId = toMap.NameId,
                Name = toMap.Name.Map(uniqueRecordId, language, parameters),
                Books = toMap.Books?.Select(x => x.Map(uniqueRecordId, language, parameters)).ToList(),
            };

            return mapped;
        }
    }
    public static class ParagraphEntity_ParagraphContractMapper
    {
        public static global::Translators.Database.Entities.ParagraphEntity Map(this global::Translators.Contracts.Common.ParagraphContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Database.Entities.ParagraphEntity()
            {
                Id = toMap.Id,
                Number = toMap.Number,
                AnotherValue = toMap.AnotherValue,
                PageId = toMap.PageId,
                Words = toMap.Words?.Select(x => x.Map(uniqueRecordId, language, parameters)).ToList(),
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.ParagraphContract Map(this global::Translators.Database.Entities.ParagraphEntity toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.ParagraphContract()
            {
                Id = toMap.Id,
                Number = toMap.Number,
                AnotherValue = toMap.AnotherValue,
                PageId = toMap.PageId,
                Words = toMap.Words?.Select(x => x.Map(uniqueRecordId, language, parameters)).ToList(),
            };

            return mapped;
        }
    }
    public static class WordEntity_WordContractMapper
    {
        public static global::Translators.Database.Entities.WordEntity Map(this global::Translators.Contracts.Common.WordContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Database.Entities.WordEntity()
            {
                Id = toMap.Id,
                Index = toMap.Index,
                ValueId = toMap.ValueId,
                Value = toMap.Value.Map(uniqueRecordId, language, parameters),
                ParagraphId = toMap.ParagraphId,
                WordLetters = toMap.WordLetters?.Select(x => x.Map(uniqueRecordId, language, parameters)).ToList(),
                WordRoots = toMap.WordRoots?.Select(x => x.Map(uniqueRecordId, language, parameters)).ToList(),
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.WordContract Map(this global::Translators.Database.Entities.WordEntity toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.WordContract()
            {
                Id = toMap.Id,
                Index = toMap.Index,
                ValueId = toMap.ValueId,
                Value = toMap.Value.Map(uniqueRecordId, language, parameters),
                ParagraphId = toMap.ParagraphId,
                WordLetters = toMap.WordLetters?.Select(x => x.Map(uniqueRecordId, language, parameters)).ToList(),
                WordRoots = toMap.WordRoots?.Select(x => x.Map(uniqueRecordId, language, parameters)).ToList(),
            };

            return mapped;
        }
    }
    public static class WordLetterEntity_WordLetterContractMapper
    {
        public static global::Translators.Database.Entities.WordLetterEntity Map(this global::Translators.Contracts.Common.WordLetterContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Database.Entities.WordLetterEntity()
            {
                Id = toMap.Id,
                Value = toMap.Value,
                WordId = toMap.WordId,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.WordLetterContract Map(this global::Translators.Database.Entities.WordLetterEntity toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.WordLetterContract()
            {
                Id = toMap.Id,
                Value = toMap.Value,
                WordId = toMap.WordId,
            };

            return mapped;
        }
    }
    public static class WordRootEntity_WordRootContractMapper
    {
        public static global::Translators.Database.Entities.WordRootEntity Map(this global::Translators.Contracts.Common.WordRootContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Database.Entities.WordRootEntity()
            {
                Id = toMap.Id,
                Value = toMap.Value,
                WordId = toMap.WordId,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.WordRootContract Map(this global::Translators.Database.Entities.WordRootEntity toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.WordRootContract()
            {
                Id = toMap.Id,
                Value = toMap.Value,
                WordId = toMap.WordId,
            };

            return mapped;
        }
    }
}

