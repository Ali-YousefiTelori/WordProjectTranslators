using BinaryGo.Runtime.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Schemas;
using Translators.Schemas.Bases;

namespace Translators.Shared.FileVersionControl
{
    public class SchemaVersionControl
    {
        public static SchemaVersionControl Current { get; set; } = new();
        Dictionary<string, object> LoadedSchemas { get; set; } = new();
        TranslatorSerializer Serializer { get; set; } = new();
        static readonly TypeHelper TypeHelper = new();

        const string DefaultCustomKey = "Default";
        protected string Root { get; set; }
        /// <summary>
        /// unique hash key for types, when any type got changes, this will change
        /// we used this key for root folder of versions
        /// </summary>
        public string UniqueVersionHashedKey { get; set; }

        public SchemaVersionControl(string root = null)
        {
            Root = root;
            if (Root == null)
                Root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SchemaVersionControl");
        }

        string GetTypeHash(string customKey, string typeHash)
        {
            return $"{customKey},{typeHash}";
        }

        string GetTypeHash(string customKey, Type type)
        {
            var hash = TypeHelper.GetTypeUniqueHash(type);
            return GetTypeHash(customKey, hash);
        }

        string GetFilePath(string customKey, Type type, string uniqueVersion = null)
        {
            var hash = GetTypeHash(customKey, type);
            var directory = uniqueVersion == null ? Root : Path.Combine(Root, uniqueVersion);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            string fileName = $"{hash}.zip";
            return uniqueVersion == null ? Path.Combine(Root, fileName) : Path.Combine(Root, uniqueVersion, fileName);
        }

        public Task<MessageContract<TSchema>> LoadSchema<TSchema>(string customKey = DefaultCustomKey)
        {
            return LoadSchema<TSchema>(null, customKey);
        }

        public async Task<MessageContract<TSchema>> LoadSchema<TSchema>(string uniqueVersion, string customKey = DefaultCustomKey)
        {
            var path = GetFilePath(customKey, typeof(TSchema), uniqueVersion);
            if (!File.Exists(path))
                return FailedReasonType.NotFound;
#if (NET45 || NETSTANDARD2_0)
            var bytes = File.ReadAllBytes(path);
#else
            var bytes = await File.ReadAllBytesAsync(path);
#endif
            return Serializer.DeSerialize<TSchema>(bytes);
        }

        public async Task SaveSchema<TSchema>(TSchema data, string customKey = DefaultCustomKey, bool doOverride = false)
        {
            var path = GetFilePath(customKey, typeof(TSchema), UniqueVersionHashedKey);
            if (File.Exists(path) && !doOverride)
                return;
            var bytes = Serializer.Serialize(data);
#if (NET45 || NETSTANDARD2_0)
            File.WriteAllBytes(path, bytes);
#else
            await File.WriteAllBytesAsync(path, bytes);
#endif
        }

        public async Task LoadAll()
        {
            await RegisterTypes(true);
        }

        public async Task RegisterTypes(bool doesLoadSchema)
        {
            TypeHelper typeHelper = new TypeHelper();
            StringBuilder builder = new StringBuilder();

            await GetTypeHash<AudioSchema>(builder, doesLoadSchema);
            await GetTypeHash<BookSchemaBase>(builder, doesLoadSchema);
            await GetTypeHash<CatalogSchemaBase>(builder, doesLoadSchema);
            await GetTypeHash<CategorySchemaBase>(builder, doesLoadSchema);
            await GetTypeHash<FileSchema>(builder, doesLoadSchema);
            await GetTypeHash<LanguageSchema>(builder, doesLoadSchema);
            await GetTypeHash<LinkGroupSchema>(builder, doesLoadSchema);
            await GetTypeHash<PageSchema>(builder, doesLoadSchema);
            await GetTypeHash<ParagraphSchema>(builder, doesLoadSchema);
            await GetTypeHash<TranslatorSchema>(builder, doesLoadSchema);
            await GetTypeHash<ValueSchema>(builder, doesLoadSchema);
            await GetTypeHash<WordSchema>(builder, doesLoadSchema);
            await GetTypeHash<WordLetterSchema>(builder, doesLoadSchema);
            await GetTypeHash<WordRootSchema>(builder, doesLoadSchema);

            //GetTypeHash<AudioFileContract>(typeHelper, builder);
            //GetTypeHash<BookContract>(typeHelper, builder);
            //GetTypeHash<CatalogContract>(typeHelper, builder);
            //GetTypeHash<CategoryContract>(typeHelper, builder);
            //GetTypeHash<FileContract>(typeHelper, builder);
            //GetTypeHash<LanguageContract>(typeHelper, builder);
            //GetTypeHash<LinkGroupContract>(typeHelper, builder);
            //GetTypeHash<MessageContract>(typeHelper, builder);
            //GetTypeHash<ErrorContract>(typeHelper, builder);
            //GetTypeHash(typeof(MessageContract<>), typeHelper, builder);
            //GetTypeHash<PageContract>(typeHelper, builder);
            //GetTypeHash<ParagraphContract>(typeHelper, builder);
            //GetTypeHash<SearchValueContract>(typeHelper, builder);
            //GetTypeHash<TranslatorContract>(typeHelper, builder);
            //GetTypeHash<UserReadingContract>(typeHelper, builder);
            //GetTypeHash<ValidationContract>(typeHelper, builder);
            //GetTypeHash<ValueContract>(typeHelper, builder);
            //GetTypeHash<WordContract>(typeHelper, builder);
            //GetTypeHash<WordLetterContract>(typeHelper, builder);
            //GetTypeHash<WordRootContract>(typeHelper, builder);
            //GetTypeHash<UserContract>(typeHelper, builder);
            //GetTypeHash<SimpleFileContract>(typeHelper, builder);
            //GetTypeHash<BookServiceModelsContract>(typeHelper, builder);
            //GetTypeHash<SimpleParagraphContract>(typeHelper, builder);
            //GetTypeHash<SimpleValueContract>(typeHelper, builder);
            //GetTypeHash<AppVersionResponseContract>(typeHelper, builder);
            //GetTypeHash<PageResponseContract>(typeHelper, builder);

            UniqueVersionHashedKey = typeHelper.GetSHA1Hash(builder.ToString());
        }

        async Task GetTypeHash<T>(StringBuilder builder, bool doesLoadSchema, string customKey = DefaultCustomKey)
        {
            var hash = GetTypeHash(customKey, typeof(Type));
            builder.Append(hash);
            if (doesLoadSchema)
            {
                var result = await LoadSchema<T>(customKey);
                if (result)
                {
                    LoadedSchemas.Add(hash, result.Result);
                }
            }
        }
    }
}
