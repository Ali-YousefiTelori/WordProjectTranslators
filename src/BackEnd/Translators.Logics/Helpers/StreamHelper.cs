using SignalGo.Shared.Models;

namespace Translators.Helpers
{
    public static class StreamHelper
    {
        public static async Task<byte[]> ReadAllBytes(BaseStreamInfo streamInfo)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                byte[] readBytes = new byte[1024 * 512];
                long writed = 0;
                long length = streamInfo.Length.Value;
                while (true)
                {
                    int readCount;
                    try
                    {
                        if (readBytes.Length > length - writed)
                            readBytes = new byte[length - writed];
                        readCount = await streamInfo.Stream.ReadAsync(readBytes, readBytes.Length);
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
    }
}
