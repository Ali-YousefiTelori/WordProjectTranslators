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
                Names = toMap.Names?.Select(x => ValueEntity_ValueContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                Catalogs = toMap.Catalogs?.Select(x => CatalogEntity_CatalogContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                Id = toMap.Id,
                CategoryId = toMap.CategoryId,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.BookContract Map(this global::Translators.Database.Entities.BookEntity toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.BookContract()
            {
                Names = toMap.Names?.Select(x => ValueEntity_ValueContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                Catalogs = toMap.Catalogs?.Select(x => CatalogEntity_CatalogContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                Id = toMap.Id,
                CategoryId = toMap.CategoryId,
            };

            return mapped;
        }
    }
    public static class ValueEntity_ValueContractMapper
    {
        public static global::Translators.Database.Entities.ValueEntity Map(this global::Translators.Contracts.Common.ValueContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Database.Entities.ValueEntity()
            {
                Language = LanguageEntity_LanguageContractMapper.Map(toMap.Language, uniqueRecordId, language, parameters),
                Translator = TranslatorEntity_TranslatorContractMapper.Map(toMap.Translator, uniqueRecordId, language, parameters),
                Id = toMap.Id,
                IsMain = toMap.IsMain,
                IsTransliteration = toMap.IsTransliteration,
                Value = toMap.Value,
                SearchValue = toMap.SearchValue,
                LanguageId = toMap.LanguageId,
                TranslatorId = toMap.TranslatorId,
                TranslatorNameId = toMap.TranslatorNameId,
                BookNameId = toMap.BookNameId,
                CategoryNameId = toMap.CategoryNameId,
                CatalogNameId = toMap.CatalogNameId,
                WordValueId = toMap.WordValueId,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.ValueContract Map(this global::Translators.Database.Entities.ValueEntity toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.ValueContract()
            {
                Language = LanguageEntity_LanguageContractMapper.Map(toMap.Language, uniqueRecordId, language, parameters),
                Translator = TranslatorEntity_TranslatorContractMapper.Map(toMap.Translator, uniqueRecordId, language, parameters),
                Id = toMap.Id,
                IsMain = toMap.IsMain,
                IsTransliteration = toMap.IsTransliteration,
                Value = toMap.Value,
                SearchValue = toMap.SearchValue,
                LanguageId = toMap.LanguageId,
                TranslatorId = toMap.TranslatorId,
                TranslatorNameId = toMap.TranslatorNameId,
                BookNameId = toMap.BookNameId,
                CategoryNameId = toMap.CategoryNameId,
                CatalogNameId = toMap.CatalogNameId,
                WordValueId = toMap.WordValueId,
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
                Names = toMap.Names?.Select(x => ValueEntity_ValueContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                Pages = toMap.Pages?.Select(x => PageEntity_PageContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                Id = toMap.Id,
                Number = toMap.Number,
                StartPageNumber = toMap.StartPageNumber,
                BookId = toMap.BookId,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.CatalogContract Map(this global::Translators.Database.Entities.CatalogEntity toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.CatalogContract()
            {
                Names = toMap.Names?.Select(x => ValueEntity_ValueContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                Pages = toMap.Pages?.Select(x => PageEntity_PageContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                Id = toMap.Id,
                Number = toMap.Number,
                StartPageNumber = toMap.StartPageNumber,
                BookId = toMap.BookId,
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
                Paragraphs = toMap.Paragraphs?.Select(x => ParagraphEntity_ParagraphContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                Audios = toMap.AudioFiles?.Select(x => AudioEntity_AudioFileContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                Id = toMap.Id,
                Number = toMap.Number,
                CatalogId = toMap.CatalogId,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.PageContract Map(this global::Translators.Database.Entities.PageEntity toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.PageContract()
            {
                Paragraphs = toMap.Paragraphs?.Select(x => ParagraphEntity_ParagraphContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                AudioFiles = toMap.Audios?.Select(x => AudioEntity_AudioFileContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                Id = toMap.Id,
                Number = toMap.Number,
                CatalogId = toMap.CatalogId,
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
                Names = toMap.Names?.Select(x => ValueEntity_ValueContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                Books = toMap.Books?.Select(x => BookEntity_BookContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                Id = toMap.Id,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.CategoryContract Map(this global::Translators.Database.Entities.CategoryEntity toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.CategoryContract()
            {
                Names = toMap.Names?.Select(x => ValueEntity_ValueContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                Books = toMap.Books?.Select(x => BookEntity_BookContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                Id = toMap.Id,
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
                Words = toMap.Words?.Select(x => WordEntity_WordContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                Audios = toMap.AudioFiles?.Select(x => AudioEntity_AudioFileContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                Id = toMap.Id,
                Number = toMap.Number,
                AnotherValue = toMap.AnotherValue,
                PageId = toMap.PageId,
                CatalogId = toMap.CatalogId,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.ParagraphContract Map(this global::Translators.Database.Entities.ParagraphEntity toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.ParagraphContract()
            {
                HasLink = toMap.LinkParagraphs?.Count > 0 || toMap.LinkParagraphs?.Count > 0,
                Words = toMap.Words?.Select(x => WordEntity_WordContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                AudioFiles = toMap.Audios?.Select(x => AudioEntity_AudioFileContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                Id = toMap.Id,
                Number = toMap.Number,
                AnotherValue = toMap.AnotherValue,
                PageId = toMap.PageId,
                CatalogId = toMap.CatalogId,
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
                Values = toMap.Values?.Select(x => ValueEntity_ValueContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                WordLetters = toMap.WordLetters?.Select(x => WordLetterEntity_WordLetterContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                WordRoots = toMap.WordRoots?.Select(x => WordRootEntity_WordRootContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                Id = toMap.Id,
                Index = toMap.Index,
                ParagraphId = toMap.ParagraphId,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.WordContract Map(this global::Translators.Database.Entities.WordEntity toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.WordContract()
            {
                Values = toMap.Values?.Select(x => ValueEntity_ValueContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                WordLetters = toMap.WordLetters?.Select(x => WordLetterEntity_WordLetterContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                WordRoots = toMap.WordRoots?.Select(x => WordRootEntity_WordRootContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                Id = toMap.Id,
                Index = toMap.Index,
                ParagraphId = toMap.ParagraphId,
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
    public static class TranslatorEntity_TranslatorContractMapper
    {
        public static global::Translators.Database.Entities.TranslatorEntity Map(this global::Translators.Contracts.Common.TranslatorContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Database.Entities.TranslatorEntity()
            {
                Names = toMap.Names?.Select(x => ValueEntity_ValueContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                Id = toMap.Id,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.TranslatorContract Map(this global::Translators.Database.Entities.TranslatorEntity toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.TranslatorContract()
            {
                Names = toMap.Names?.Select(x => ValueEntity_ValueContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                Id = toMap.Id,
            };

            return mapped;
        }
    }
    public static class LogEntity_LogContractMapper
    {
        public static global::Translators.Database.Entities.LogEntity Map(this global::Translators.Contracts.Common.LogContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Database.Entities.LogEntity()
            {
                Id = toMap.Id,
                LogTrace = toMap.LogTrace,
                AppVersion = toMap.AppVersion,
                Session = toMap.Session,
                DeviceDescription = toMap.DeviceDescription,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.LogContract Map(this global::Translators.Database.Entities.LogEntity toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.LogContract()
            {
                Id = toMap.Id,
                LogTrace = toMap.LogTrace,
                AppVersion = toMap.AppVersion,
                Session = toMap.Session,
                DeviceDescription = toMap.DeviceDescription,
            };

            return mapped;
        }
    }
    public static class AudioEntity_AudioFileContractMapper
    {
        public static global::Translators.Database.Entities.AudioEntity Map(this global::Translators.Contracts.Common.AudioFileContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Database.Entities.AudioEntity()
            {
                Id = toMap.Id,
                IsMain = toMap.IsMain,
                FileName = toMap.FileName,
                Password = toMap.Password,
                DurationTicks = toMap.DurationTicks,
                PageId = toMap.PageId,
                LanguageId = toMap.LanguageId,
                ParagraphId = toMap.ParagraphId,
                AudioReaderId = toMap.AudioReaderId,
                TranslatorId = toMap.TranslatorId,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.AudioFileContract Map(this global::Translators.Database.Entities.AudioEntity toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.AudioFileContract()
            {
                IsMain = toMap.IsMain,
                FileName = toMap.FileName,
                DurationTicks = toMap.DurationTicks,
                PageId = toMap.PageId,
                LanguageId = toMap.LanguageId,
                TranslatorId = toMap.TranslatorId,
                ParagraphId = toMap.ParagraphId,
                AudioReaderId = toMap.AudioReaderId,
                Id = toMap.Id,
                Password = toMap.Password,
            };

            return mapped;
        }
    }
    public static class ParagraphContract_SimpleParagraphContractMapper
    {
        public static global::Translators.Contracts.Common.ParagraphContract Map(this global::Translators.Contracts.Common.Paragraphs.SimpleParagraphContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.ParagraphContract()
            {
                HasLink = toMap.HasLink,
                Words = toMap.MainWords?.Select(x => WordContract_SimpleWordContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                AudioFiles = toMap.AudioFiles?.Select(x => AudioFileContract_SimpleFileContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                Id = toMap.Id,
                Number = toMap.Number,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.Paragraphs.SimpleParagraphContract Map(this global::Translators.Contracts.Common.ParagraphContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.Paragraphs.SimpleParagraphContract()
            {
                Id = toMap.Id,
                Number = toMap.Number,
                HasLink = toMap.HasLink,
                MainWords = toMap.Words?.Select(x => WordContract_SimpleWordContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                TranslatedValue = string.Join(' ', toMap.Words?.SelectMany(x => x.Values).Where(x => !x.IsMain && !x.IsTransliteration).Select(x => x.Value)),
                AudioFiles = toMap.AudioFiles?.Select(x => AudioFileContract_SimpleFileContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
            };

            return mapped;
        }
    }
    public static class WordContract_SimpleWordContractMapper
    {
        public static global::Translators.Contracts.Common.WordContract Map(this global::Translators.Contracts.Common.Words.SimpleWordContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.WordContract()
            {
                Id = toMap.Id,
                Index = toMap.Index,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.Words.SimpleWordContract Map(this global::Translators.Contracts.Common.WordContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var value = toMap.Values.First();
            var mapped = new global::Translators.Contracts.Common.Words.SimpleWordContract()
            {
                Id = toMap.Id,
                Index = toMap.Index,
                IsTransliteration = value.IsTransliteration,
                Value = value.Value,
                LanguageId = value.LanguageId,
            };

            return mapped;
        }
    }
    public static class AudioFileContract_SimpleFileContractMapper
    {
        public static global::Translators.Contracts.Common.AudioFileContract Map(this global::Translators.Contracts.Common.Files.SimpleFileContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.AudioFileContract()
            {
                IsMain = toMap.IsMain,
                FileName = toMap.FileName,
                DurationTicks = toMap.DurationTicks,
                LanguageId = toMap.LanguageId,
                TranslatorId = toMap.TranslatorId,
                AudioReaderId = toMap.AudioReaderId,
                Id = toMap.Id,
                Password = toMap.Password,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.Files.SimpleFileContract Map(this global::Translators.Contracts.Common.AudioFileContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.Files.SimpleFileContract()
            {
                IsMain = toMap.IsMain,
                FileName = toMap.FileName,
                DurationTicks = toMap.DurationTicks,
                LanguageId = toMap.LanguageId,
                TranslatorId = toMap.TranslatorId,
                AudioReaderId = toMap.AudioReaderId,
                Id = toMap.Id,
                Password = toMap.Password,
            };

            return mapped;
        }
    }
    public static class BookSchemaBase_BookContractMapper
    {
        public static global::Translators.Schemas.Bases.BookSchemaBase Map(this global::Translators.Contracts.Common.BookContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Schemas.Bases.BookSchemaBase()
            {
                Id = toMap.Id,
                IsHidden = toMap.IsHidden,
                CategoryId = toMap.CategoryId,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.BookContract Map(this global::Translators.Schemas.Bases.BookSchemaBase toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.BookContract()
            {
                Id = toMap.Id,
                IsHidden = toMap.IsHidden,
                CategoryId = toMap.CategoryId,
            };

            return mapped;
        }
    }
    public static class AudioSchema_AudioFileContractMapper
    {
        public static global::Translators.Schemas.AudioSchema Map(this global::Translators.Contracts.Common.AudioFileContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Schemas.AudioSchema()
            {
                Id = toMap.Id,
                IsMain = toMap.IsMain,
                FileName = toMap.FileName,
                Password = toMap.Password,
                DurationTicks = toMap.DurationTicks,
                PageId = toMap.PageId,
                LanguageId = toMap.LanguageId,
                ParagraphId = toMap.ParagraphId,
                AudioReaderId = toMap.AudioReaderId,
                TranslatorId = toMap.TranslatorId,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.AudioFileContract Map(this global::Translators.Schemas.AudioSchema toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.AudioFileContract()
            {
                IsMain = toMap.IsMain,
                FileName = toMap.FileName,
                DurationTicks = toMap.DurationTicks,
                PageId = toMap.PageId,
                LanguageId = toMap.LanguageId,
                TranslatorId = toMap.TranslatorId,
                ParagraphId = toMap.ParagraphId,
                AudioReaderId = toMap.AudioReaderId,
                Id = toMap.Id,
                Password = toMap.Password,
            };

            return mapped;
        }
    }
    public static class CatalogSchemaBase_CatalogContractMapper
    {
        public static global::Translators.Schemas.Bases.CatalogSchemaBase Map(this global::Translators.Contracts.Common.CatalogContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Schemas.Bases.CatalogSchemaBase()
            {
                Id = toMap.Id,
                Number = toMap.Number,
                StartPageNumber = toMap.StartPageNumber,
                BookId = toMap.BookId,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.CatalogContract Map(this global::Translators.Schemas.Bases.CatalogSchemaBase toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.CatalogContract()
            {
                Id = toMap.Id,
                Number = toMap.Number,
                StartPageNumber = toMap.StartPageNumber,
                BookId = toMap.BookId,
            };

            return mapped;
        }
    }
    public static class CategorySchemaBase_CategoryContractMapper
    {
        public static global::Translators.Schemas.Bases.CategorySchemaBase Map(this global::Translators.Contracts.Common.CategoryContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Schemas.Bases.CategorySchemaBase()
            {
                Id = toMap.Id,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.CategoryContract Map(this global::Translators.Schemas.Bases.CategorySchemaBase toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.CategoryContract()
            {
                Id = toMap.Id,
            };

            return mapped;
        }
    }
    public static class FileSchema_FileContractMapper
    {
        public static global::Translators.Schemas.FileSchema Map(this global::Translators.Contracts.Common.FileContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Schemas.FileSchema()
            {
                Id = toMap.Id,
                Password = toMap.Password,
                Url = toMap.Url,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.FileContract Map(this global::Translators.Schemas.FileSchema toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.FileContract()
            {
                Id = toMap.Id,
                Password = toMap.Password,
                Url = toMap.Url,
            };

            return mapped;
        }
    }
    public static class LanguageSchema_LanguageContractMapper
    {
        public static global::Translators.Schemas.LanguageSchema Map(this global::Translators.Contracts.Common.LanguageContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Schemas.LanguageSchema()
            {
                Id = toMap.Id,
                Name = toMap.Name,
                Code = toMap.Code,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.LanguageContract Map(this global::Translators.Schemas.LanguageSchema toMap, string uniqueRecordId, string language, object[] parameters)
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
    public static class LinkGroupSchema_LinkGroupContractMapper
    {
        public static global::Translators.Schemas.LinkGroupSchema Map(this global::Translators.Contracts.Common.LinkGroupContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Schemas.LinkGroupSchema()
            {
                Id = toMap.Id,
                Title = toMap.Title,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.LinkGroupContract Map(this global::Translators.Schemas.LinkGroupSchema toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.LinkGroupContract()
            {
                Id = toMap.Id,
                Title = toMap.Title,
            };

            return mapped;
        }
    }
    public static class LinkGroupEntity_LinkGroupContractMapper
    {
        public static global::Translators.Database.Entities.LinkGroupEntity Map(this global::Translators.Contracts.Common.LinkGroupContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Database.Entities.LinkGroupEntity()
            {
                Id = toMap.Id,
                Title = toMap.Title,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.LinkGroupContract Map(this global::Translators.Database.Entities.LinkGroupEntity toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.LinkGroupContract()
            {
                Id = toMap.Id,
                Title = toMap.Title,
            };

            return mapped;
        }
    }
    public static class PageSchema_PageContractMapper
    {
        public static global::Translators.Schemas.PageSchema Map(this global::Translators.Contracts.Common.PageContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Schemas.PageSchema()
            {
                Id = toMap.Id,
                Number = toMap.Number,
                CatalogId = toMap.CatalogId,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.PageContract Map(this global::Translators.Schemas.PageSchema toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.PageContract()
            {
                Id = toMap.Id,
                Number = toMap.Number,
                CatalogId = toMap.CatalogId,
            };

            return mapped;
        }
    }
    public static class ParagraphSchema_ParagraphContractMapper
    {
        public static global::Translators.Schemas.ParagraphSchema Map(this global::Translators.Contracts.Common.ParagraphContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Schemas.ParagraphSchema()
            {
                Id = toMap.Id,
                Number = toMap.Number,
                AnotherValue = toMap.AnotherValue,
                PageId = toMap.PageId,
                CatalogId = toMap.CatalogId,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.ParagraphContract Map(this global::Translators.Schemas.ParagraphSchema toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.ParagraphContract()
            {
                Id = toMap.Id,
                Number = toMap.Number,
                AnotherValue = toMap.AnotherValue,
                PageId = toMap.PageId,
                CatalogId = toMap.CatalogId,
            };

            return mapped;
        }
    }
    public static class TranslatorSchema_TranslatorContractMapper
    {
        public static global::Translators.Schemas.TranslatorSchema Map(this global::Translators.Contracts.Common.TranslatorContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Schemas.TranslatorSchema()
            {
                Names = toMap.Names?.Select(x => ValueSchema_ValueContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                Id = toMap.Id,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.TranslatorContract Map(this global::Translators.Schemas.TranslatorSchema toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.TranslatorContract()
            {
                Names = toMap.Names?.Select(x => ValueSchema_ValueContractMapper.Map(x, uniqueRecordId, language, parameters)).ToList(),
                Id = toMap.Id,
            };

            return mapped;
        }
    }
    public static class ValueSchema_ValueContractMapper
    {
        public static global::Translators.Schemas.ValueSchema Map(this global::Translators.Contracts.Common.ValueContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Schemas.ValueSchema()
            {
                Id = toMap.Id,
                IsMain = toMap.IsMain,
                IsTransliteration = toMap.IsTransliteration,
                Value = toMap.Value,
                SearchValue = toMap.SearchValue,
                LanguageId = toMap.LanguageId,
                TranslatorId = toMap.TranslatorId,
                TranslatorNameId = toMap.TranslatorNameId,
                BookNameId = toMap.BookNameId,
                CategoryNameId = toMap.CategoryNameId,
                CatalogNameId = toMap.CatalogNameId,
                WordValueId = toMap.WordValueId,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.ValueContract Map(this global::Translators.Schemas.ValueSchema toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.ValueContract()
            {
                Id = toMap.Id,
                IsMain = toMap.IsMain,
                IsTransliteration = toMap.IsTransliteration,
                Value = toMap.Value,
                SearchValue = toMap.SearchValue,
                LanguageId = toMap.LanguageId,
                TranslatorId = toMap.TranslatorId,
                TranslatorNameId = toMap.TranslatorNameId,
                BookNameId = toMap.BookNameId,
                CategoryNameId = toMap.CategoryNameId,
                CatalogNameId = toMap.CatalogNameId,
                WordValueId = toMap.WordValueId,
            };

            return mapped;
        }
    }
    public static class WordSchema_WordContractMapper
    {
        public static global::Translators.Schemas.WordSchema Map(this global::Translators.Contracts.Common.WordContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Schemas.WordSchema()
            {
                Id = toMap.Id,
                Index = toMap.Index,
                ParagraphId = toMap.ParagraphId,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.WordContract Map(this global::Translators.Schemas.WordSchema toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Contracts.Common.WordContract()
            {
                Id = toMap.Id,
                Index = toMap.Index,
                ParagraphId = toMap.ParagraphId,
            };

            return mapped;
        }
    }
    public static class WordLetterSchema_WordLetterContractMapper
    {
        public static global::Translators.Schemas.WordLetterSchema Map(this global::Translators.Contracts.Common.WordLetterContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Schemas.WordLetterSchema()
            {
                Id = toMap.Id,
                Value = toMap.Value,
                WordId = toMap.WordId,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.WordLetterContract Map(this global::Translators.Schemas.WordLetterSchema toMap, string uniqueRecordId, string language, object[] parameters)
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
    public static class WordRootSchema_WordRootContractMapper
    {
        public static global::Translators.Schemas.WordRootSchema Map(this global::Translators.Contracts.Common.WordRootContract toMap, string uniqueRecordId, string language, object[] parameters)
        {
            if (toMap == default)
                return default;
            var mapped = new global::Translators.Schemas.WordRootSchema()
            {
                Id = toMap.Id,
                Value = toMap.Value,
                WordId = toMap.WordId,
            };

            return mapped;
        }
        public static global::Translators.Contracts.Common.WordRootContract Map(this global::Translators.Schemas.WordRootSchema toMap, string uniqueRecordId, string language, object[] parameters)
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

