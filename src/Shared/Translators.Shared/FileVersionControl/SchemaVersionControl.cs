using BinaryGo.Runtime.Helpers;
using EasyMicroservices.FileManager.Interfaces;
using EasyMicroservices.FileManager.Providers.PathProviders;
using EasyMicroservices.Serialization.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
        ITextSerialization _serializer { get; set; }

        public SchemaVersionControl(ITextSerialization serializer, IPathProvider pathProvider, IFileManagerProvider fileManager, IDirectoryManagerProvider directoryManager, string root = null)
        {
            _pathProvider = pathProvider;
            _fileManager = fileManager;
            _directoryManager = directoryManager;
            _serializer = serializer;
            Root = root;
            if (Root == null)
                Root = pathProvider.Combine(AppDomain.CurrentDomain.BaseDirectory, "SchemaVersionControl");
        }

        public static void Initialize()
        {
            var pathyProvider = new SystemPathProvider();
            var directory = new EasyMicroservices.FileManager.Providers.DirectoryProviders.DiskDirectoryProvider("VersionFiles", pathyProvider);
            var file = new EasyMicroservices.FileManager.Providers.FileProviders.DiskFileProvider(directory);
            var serializer = new EasyMicroservices.Serialization.Newtonsoft.Json.Providers.NewtonsoftJsonProvider();
            Current = new SchemaVersionControl(serializer, pathyProvider, file, directory);
        }

        string GetTypeHash(string typeName, string typeHash)
        {
            return $"{typeName},{typeHash}";
        }

        string GetTypeHash(Type type)
        {
            var hash = TypeHelper.GetTypeUniqueHash(type);
            return GetTypeHash(GetTypeName(type), hash);
        }

        string GetTypeName(Type type)
        {
            if (type.Name.Contains('`') || type.Name.Contains('['))
            {
                var name = CleanName(type.Name);
                if (type.GenericTypeArguments.Length > 0)
                    return $"{name}_{CleanName(GetGenericNames(type.GenericTypeArguments))}";
                if (type.HasElementType)
                    return $"{name}_Array";
                return name;
            }
            return type.Name;
        }

        string CleanName(string name)
        {
            return name.Split('`').FirstOrDefault().Replace("[", "").Replace("]", "");
        }

        string GetGenericNames(Type[] types)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var type in types)
            {
                stringBuilder.Append(CleanName(type.Name));
                stringBuilder.Append('_');
            }
            return stringBuilder.ToString();
        }

        async Task<string> GetFilePath(string customKey, string typeHash, string uniqueVersion = null)
        {
            var directory = uniqueVersion == null ? _pathProvider.Combine(Root, UniqueVersionHashedKey) : _pathProvider.Combine(Root, uniqueVersion);
            if (!await _directoryManager.IsExistDirectoryAsync(directory))
                await _directoryManager.CreateDirectoryAsync(directory);
            string fileName = $"{customKey},{typeHash}.zip";
            return _pathProvider.Combine(directory, fileName);
        }

        public Task<MessageContract<TSchema>> LoadSchema<TSchema>(string customKey = DefaultCustomKey, string uniqueVersion = null)
        {
            var hash = GetTypeHash(typeof(TSchema));
            return LoadSchema<TSchema>(hash, customKey, uniqueVersion);
        }

        public async Task<MessageContract<TSchema>> LoadSchema<TSchema>(string typeHash, string customKey = DefaultCustomKey, string uniqueVersion = null)
        {
            var contract = await GetSchemaBytes(typeHash, customKey, uniqueVersion);
            if (contract)
                return _serializer.Deserialize<TSchema>(Encoding.UTF8.GetString(contract.Result));
            return contract.ToContract<TSchema>();
        }

        public async Task<MessageContract<TSchema>> SaveAndUpdateSchema<TSchema>(TSchema saveData, string customKey = DefaultCustomKey)
        {
            await SaveSchema(saveData, customKey, true);
            return saveData;
        }

        async Task<MessageContract<byte[]>> GetSchemaBytes(string typeHash, string customKey = DefaultCustomKey, string uniqueVersion = null)
        {
            var path = await GetFilePath(customKey, typeHash, uniqueVersion);
            if (!await _fileManager.IsExistFileAsync(path))
                return FailedReasonType.NotFound;
            var bytes = await _fileManager.ReadAllBytesAsync(path);
            return bytes;
        }

        public async Task SaveSchema<TSchema>(TSchema data, string customKey = DefaultCustomKey, bool doOverride = false)
        {
            var hash = GetTypeHash(typeof(TSchema));
            var path = await GetFilePath(customKey, hash, UniqueVersionHashedKey);
            if (await _fileManager.IsExistFileAsync(path) && !doOverride)
                return;
            var text = _serializer.Serialize(data);
            await _fileManager.WriteAllBytesAsync(path, Encoding.UTF8.GetBytes(text));
        }

        public async Task LoadAll()
        {
            await RegisterTypes(true);
        }

        public async Task RegisterTypes(bool doesLoadSchema)
        {
            TypeHelper typeHelper = new TypeHelper();
            StringBuilder builder = new StringBuilder();

            GetTypeHash<AudioSchema>(builder, doesLoadSchema);
            GetTypeHash<BookSchemaBase>(builder, doesLoadSchema);
            GetTypeHash<CatalogSchemaBase>(builder, doesLoadSchema);
            GetTypeHash<CategorySchemaBase>(builder, doesLoadSchema);
            GetTypeHash<FileSchema>(builder, doesLoadSchema);
            GetTypeHash<LanguageSchema>(builder, doesLoadSchema);
            GetTypeHash<LinkGroupSchema>(builder, doesLoadSchema);
            GetTypeHash<PageSchema>(builder, doesLoadSchema);
            GetTypeHash<ParagraphSchema>(builder, doesLoadSchema);
            GetTypeHash<TranslatorSchema>(builder, doesLoadSchema);
            GetTypeHash<ValueSchema>(builder, doesLoadSchema);
            GetTypeHash<WordSchema>(builder, doesLoadSchema);
            GetTypeHash<WordLetterSchema>(builder, doesLoadSchema);
            GetTypeHash<WordRootSchema>(builder, doesLoadSchema);

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
            if (doesLoadSchema)
            {
                var method = typeof(SchemaVersionControl).GetMethod(nameof(SaveAndUpdateSchema), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                foreach (var keyPair in RegisteredTypes)
                {
                    var callMethod = method.MakeGenericMethod(keyPair.Value);
                    var resultAwaitor = (Task)callMethod.Invoke(this, new object[] { DefaultCustomKey });
                    await resultAwaitor;
                    //var resultProperty = typeof(Task<>).MakeGenericType(typeof(MessageContract<>).MakeGenericType(keyPair.Value)).GetProperty("Result");
                    //var result = (MessageContract)resultProperty.GetValue(resultAwaitor);
                    //if (result)
                    //{
                    //    LoadedSchemas.Add(keyPair.Key, result.GetResult());
                    //}
                }
            }
        }

        Dictionary<string, Type> RegisteredTypes { get; } = new Dictionary<string, Type>();
        void GetTypeHash<T>(StringBuilder builder, bool doesLoadSchema, string customKey = DefaultCustomKey)
        {
            ExtractTypeHash<T>(builder, doesLoadSchema, customKey);
            ExtractTypeHash<List<T>>(builder, doesLoadSchema, customKey);
            ExtractTypeHash<T[]>(builder, doesLoadSchema, customKey);
        }

        void ExtractTypeHash<T>(StringBuilder builder, bool doesLoadSchema, string customKey = DefaultCustomKey)
        {
            var hash = GetTypeHash(typeof(T));
            builder.Append(hash);
            RegisteredTypes.Add(hash, typeof(T));
        }
    }
}
