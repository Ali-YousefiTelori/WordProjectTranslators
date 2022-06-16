using TranslatorsServices.Interfaces;

namespace Translators.ServiceManagers
{
    public static class TranslatorService
    {
        public static void Initialize()
        {

        }

        static string ServiceAddress { get; set; } = "http://api.noorpod.ir";//"http://localhost:9341";
        static TranslatorNoCacheHttpClient NoCacheHttpClient { get; set; } = new TranslatorNoCacheHttpClient();
        static TranslatorHttpClient CacheHttpClient { get; set; } = new TranslatorHttpClient();

        public static IBookServiceAsync GetBookServiceHttp(bool isForce)
        {
            if (isForce)
                return new TranslatorsServices.HttpServices.BookService(ServiceAddress, NoCacheHttpClient);
            else
                return new TranslatorsServices.HttpServices.BookService(ServiceAddress, CacheHttpClient);
        }

        public static IChapterServiceAsync GetChapterServiceHttp(bool isForce)
        {
            if (isForce)
                return new TranslatorsServices.HttpServices.ChapterService(ServiceAddress, NoCacheHttpClient);
            else
                return new TranslatorsServices.HttpServices.ChapterService(ServiceAddress, CacheHttpClient);
        }

        public static IPageService GetPageServiceHttp(bool isForce)
        {
            if (isForce)
                return new TranslatorsServices.HttpServices.PageService(ServiceAddress, NoCacheHttpClient);
            else
                return new TranslatorsServices.HttpServices.PageService(ServiceAddress, CacheHttpClient);
        }
    }
}
