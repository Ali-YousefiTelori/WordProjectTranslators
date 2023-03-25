using BinaryGo.Runtime.Helpers;
using EasyMicroservices.FileManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Schemas;
using Translators.Schemas.Bases;

namespace Translators.Shared.FileVersionControl
{
    public class SchemaVersionControl
    {
        public static SchemaVersionControl Current { get; set; }
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


        IPathProvider _pathProvider;
        IFileManagerProvider _fileManager;
        IDirectoryManagerProvider _directoryManager;

        public SchemaVersionControl(IPathProvider pathProvider, IFileManagerProvider fileManager, IDirectoryManagerProvider directoryManager, string root = null)
        {
            _pathProvider = pathProvider;
            _fileManager = fileManager;
            _directoryManager = directoryManager;
            Root = root;
            if (Root == null)
                Root = pathProvider.Combine(AppDomain.CurrentDomain.BaseDirectory, "SchemaVersionControl");
        }

        string GetTypeHash(string customKey, string typeName, string typeHash)
        {
            return $"{customKey},{typeName},{typeHash}";
        }

        string GetTypeHash(string customKey, Type type)
        {
            var hash = TypeHelper.GetTypeUniqueHash(type);
            return GetTypeHash(customKey, type.Name, hash);
        }

        async Task<string> GetFilePath(string customKey, Type type, string uniqueVersion = null)
        {
            var hash = GetTypeHash(customKey, type);
            var directory = uniqueVersion == null ? Root : _pathProvider.Combine(Root, uniqueVersion);
            if (!await _directoryManager.IsExistDirectoryAsync(directory))
                await _directoryManager.CreateDirectoryAsync(directory);
            string fileName = $"{hash}.zip";
            return uniqueVersion == null ? _pathProvider.Combine(Root, fileName) : _pathProvider.Combine(Root, uniqueVersion, fileName);
        }
        public async Task<MessageContract<TSchema>> LoadSchema<TSchema>(string customKey = DefaultCustomKey, string uniqueVersion = null)
        {
            var contract = await GetSchemaBytes<TSchema>(customKey, uniqueVersion);
            if (contract)
                return Serializer.DeSerialize<TSchema>(contract.Result);
            return contract.ToContract<TSchema>();
        }

        async Task<MessageContract<byte[]>> GetSchemaBytes<TSchema>(string customKey = DefaultCustomKey, string uniqueVersion = null)
        {
            var path = await GetFilePath(customKey, typeof(TSchema), uniqueVersion);
            if (!await _fileManager.IsExistFileAsync(path))
                return FailedReasonType.NotFound;
            var bytes = await _fileManager.ReadAllBytesAsync(path);
            return bytes;
        }

        public async Task SaveSchemaList<TSchema>(List<TSchema> data, string customKey = DefaultCustomKey, bool doOverride = false)
        {
            var path = await GetFilePath(customKey, typeof(TSchema), UniqueVersionHashedKey);
            if (await _fileManager.IsExistFileAsync(path) && !doOverride)
                return;
            var bytes = Serializer.Serialize(data);
            await _fileManager.WriteAllBytesAsync(path, bytes);
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
