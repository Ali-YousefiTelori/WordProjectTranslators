using BinaryGo.Binary;
using BinaryGo.Binary.Deserialize;
using System;
using System.Collections.Generic;
using System.Text;

namespace Translators.Shared.FileVersionControl
{
    public class TranslatorSerializer
    {
        public byte[] Serialize<T>(T obj)
        {
            return BinarySerializer.NormalInstance.Serialize(obj).ToArray();
        }

        public T DeSerialize<T>(Span<byte> bytes)
        {
            return BinaryDeserializer.NormalInstance.Deserialize<T>(bytes);
        }
    }
}
