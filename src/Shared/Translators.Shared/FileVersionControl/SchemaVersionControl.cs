using BinaryGo.Runtime.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;
using Translators.Contracts.Common;

namespace Translators.Shared.FileVersionControl
{
    public class SchemaVersionControl
    {
        TranslatorSerializer Serializer = new TranslatorSerializer();
        public SchemaVersionControl(string root = null)
        {
            Root = root;
            if (Root == null)
                Root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SchemaVersionControl");

            if (!Directory.Exists(Root))
                Directory.CreateDirectory(Root);
        }

        protected string Root { get; set; }

        public async Task<MessageContract<TFlat>> LoadSchema<TFlat>(int version)
        {
            TypeHelper typeHelper = new TypeHelper();
            var hash = typeHelper.GetTypeUniqueHash(typeof(TFlat));
            var path = Path.Combine(Root, version.ToString(), $"{hash}.zip");
            if (!File.Exists(path))
                return FailedReasonType.NotFound;
#if (NET45 || NETSTANDARD2_0)
            var bytes = File.ReadAllBytes(path);
#else
            var bytes = await File.ReadAllBytesAsync(path);
#endif
            return Serializer.DeSerialize<TFlat>(bytes);
        }

        public async Task SaveSchema<TFlat>(TFlat data, int version, bool doOverride = false)
        {
            TypeHelper typeHelper = new TypeHelper();
            var hash = typeHelper.GetTypeUniqueHash(typeof(TFlat));
            var directory = Path.Combine(Root, version.ToString());
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            var path = Path.Combine(directory, $"{hash}.zip");
            if (File.Exists(path) && !doOverride)
                return;
            var bytes = Serializer.Serialize(data);
#if (NET45 || NETSTANDARD2_0)
            File.WriteAllBytes(path, bytes);
#else
            await File.WriteAllBytesAsync(path, bytes);
#endif
        }
    }
}
