using SignalGo.Client;
using System.Text;
using TranslatorsServices.Interfaces;

namespace Translators.Helpers
{
    public static class TranslatorService
    {
        public static void Initialize()
        {

        }

        static string ServiceAddress { get; set; } = "http://api.noorpod.ir";//"http://localhost:9341";
        static HttpClient HttpClient { get; set; } = new HttpClient() { Encoding = Encoding.UTF8 };

        public static IBookServiceAsync BookServiceHttp
        {
            get
            {
                return new TranslatorsServices.HttpServices.BookService(ServiceAddress, HttpClient);
            }
        }

        public static IChapterServiceAsync ChapterServiceHttp
        {
            get
            {
                return new TranslatorsServices.HttpServices.ChapterService(ServiceAddress, HttpClient);
            }
        }

        public static IPageService PageServiceHttp
        {
            get
            {
                return new TranslatorsServices.HttpServices.PageService(ServiceAddress, HttpClient);
            }
        }
    }
}
