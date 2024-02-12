using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace EasyMicroservices.TranslatorsMicroservice.Helpers;
/// <summary>
/// 
/// </summary>
public class ConcurrentCompressDictionary<TKey, TValue> : IDictionary<TKey, TValue>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public TValue this[TKey key]
    {
        get
        {
            return DecompressBytesToObject<TValue>(CompressedData[key]);
        }
        set
        {
            Add(key, value);
        }
    }

    ConcurrentDictionary<TKey, byte[]> CompressedData { get; set; } = new ConcurrentDictionary<TKey, byte[]>();
    /// <summary>
    /// 
    /// </summary>
    public ICollection<TKey> Keys
    {
        get
        {
            return CompressedData.Keys;
        }
    }

    public ICollection<TValue> Values
    {
        get
        {
            return CompressedData.Select(x => DecompressBytesToObject<TValue>(x.Value)).ToList();
        }
    }

    public int Count { get; }
    public bool IsReadOnly { get; }

    public void Add(TKey key, TValue value)
    {
        CompressedData.TryAdd(key, CompressObjectToBytes(value));
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        CompressedData.TryAdd(item.Key, CompressObjectToBytes(item.Value));
    }

    public void Clear()
    {
        CompressedData.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return CompressedData.ContainsKey(item.Key);
    }

    public bool ContainsKey(TKey key)
    {
        return CompressedData.ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return CompressedData
            .ToDictionary(x => x.Key, x => DecompressBytesToObject<TValue>(x.Value))
            .GetEnumerator();
    }

    public bool Remove(TKey key)
    {
        return CompressedData.TryRemove(key, out _);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return CompressedData.TryRemove(item.Key, out _);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        if (CompressedData.TryGetValue(key, out byte[] bytes))
        {
            value = DecompressBytesToObject<TValue>(bytes);
            return true;
        }
        value = default(TValue);
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    static byte[] CompressObjectToBytes<T>(T obj)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(obj);
        var jsonBytes = Encoding.UTF8.GetBytes(json);

        using (var outputStream = new MemoryStream())
        {
            using (var gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
            {
                gzipStream.Write(jsonBytes, 0, jsonBytes.Length);
                gzipStream.Close();
                return outputStream.ToArray();
            }
        }
    }

    static T DecompressBytesToObject<T>(byte[] compressedBytes)
    {
        using (var inputStream = new MemoryStream(compressedBytes))
        {
            using (var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
            {
                using (var reader = new StreamReader(gzipStream, Encoding.UTF8))
                {
                    var json = reader.ReadToEnd();
                    return System.Text.Json.JsonSerializer.Deserialize<T>(json);
                }
            }
        }
    }
}

