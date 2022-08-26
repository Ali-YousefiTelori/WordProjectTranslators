namespace Translators.Patches
{
    public static class VoidUploader
    {
        public static async Task Upload()
        {
            TranslatorsServices.HttpServices.PageService Page = new TranslatorsServices.HttpServices.PageService("http://api.noorpod.ir/");
            for (int i = 5; i < 6; i++)
            {
                var bookId = i;
                var pages = await Page.GetPagesByBookIdAsync(bookId);
                //foreach (var file in Directory.GetFiles($@"D:\application\Audio pods\{i}").OrderBy(x => int.Parse(Path.GetFileNameWithoutExtension(x))))
                foreach (var file in Directory.GetFiles($@"D:\application\Audio pods\enjil\{(i-1)}").OrderBy(x => int.Parse(Path.GetFileNameWithoutExtension(x))))
                {
                    var page = pages.Result.FirstOrDefault(x => x.Number == int.Parse(Path.GetFileNameWithoutExtension(file)));
                    Console.WriteLine($"File uploaded for bookid {bookId} and page {file}");
                    using (var fileStream = File.OpenRead(file))
                    {
                        var result = await Page.UploadFileAsync(new SignalGo.Shared.Models.StreamInfo<long>()
                        {
                            Data = page.Id,
                            ContentType = "audio/mpeg",
                            FileName = Path.GetFileName(file),
                            Length = fileStream.Length,
                            Stream = fileStream
                        });
                    }
                }
            }
        }
    }
}
