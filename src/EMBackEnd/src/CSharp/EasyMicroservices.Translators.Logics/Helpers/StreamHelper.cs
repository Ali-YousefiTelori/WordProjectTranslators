using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Helpers
{
    public static class StreamHelper
    {
        public static async Task<byte[]> ReadAllBytes(Stream streamInfo)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                byte[] readBytes = new byte[1024 * 512];
                long writed = 0;
                long length = streamInfo.Length;
                while (true)
                {
                    int readCount;
                    try
                    {
                        if (readBytes.Length > length - writed)
                            readBytes = new byte[length - writed];
                        readCount = await streamInfo.ReadAsync(readBytes, 0, readBytes.Length);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("read exception : " + ex.ToString());
                        break;
                    }
                    if (readCount <= 0)
                        break;
                    memoryStream.Write(readBytes, 0, readCount);
                    writed += readCount;
                    if (writed == length)
                        break;
                }
                return memoryStream.ToArray();
            }
        }

        public static FileStreamResult GetOfflineCache<T>(HttpContext context, T contract, Type serviceType)
        {
            string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cache", serviceType.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            string cacheFile = Path.Combine(folder, typeof(T).Name + ".zip");
            MemoryStream stream;
            context.Response.Headers.Add("content-disposition", $"attachment; filename=\"{Path.GetFileName(cacheFile)}\"");
            context.Response.Headers.Add("Content-Type", "application/zip");
            if (File.Exists(cacheFile))
            {
                stream = new MemoryStream(File.ReadAllBytes(cacheFile));
                context.Response.Headers.Add("Content-Length", stream.Length.ToString());
                return new FileStreamResult(stream, "application/zip");
            }
            var stringResult = JsonConvert.SerializeObject(contract);
            using (var outStream = new MemoryStream())
            {
                using (var tinyStream = new GZipStream(outStream, CompressionMode.Compress))
                using (var mStream = new MemoryStream(Encoding.UTF8.GetBytes(stringResult)))
                    mStream.CopyTo(tinyStream);
                var allBytes = outStream.ToArray();
                File.WriteAllBytes(cacheFile, allBytes);
                stream = new MemoryStream(allBytes);
                stream.Seek(0, SeekOrigin.Begin);
            }
            context.Response.Headers.Add("Content-Length", stream.Length.ToString());
            return new FileStreamResult(stream, "application/zip");
        }
    }
}
