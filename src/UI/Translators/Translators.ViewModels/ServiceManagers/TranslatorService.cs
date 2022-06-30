using SignalGo.Client;
using System.Threading.Tasks;
using TranslatorsServices.Interfaces;

namespace Translators.ServiceManagers
{
    public static class TranslatorService
    {
        public static string Session { get; set; }
        public static void Initialize()
        {
            ClientProvider clientProvider = new ClientProvider();
            DuplexBookService = clientProvider.RegisterServerService<TranslatorsServices.ServerServices.BookService>(clientProvider);
            DuplexChapterService = clientProvider.RegisterServerService<TranslatorsServices.ServerServices.ChapterService>(clientProvider);
            DuplexPageService = clientProvider.RegisterServerService<TranslatorsServices.ServerServices.PageService>(clientProvider);
            DuplexHealthService = clientProvider.RegisterServerService<TranslatorsServices.ServerServices.HealthService>(clientProvider);
            DuplexAuthenticationService = clientProvider.RegisterServerService<TranslatorsServices.ServerServices.AuthenticationService>(clientProvider);

            clientProvider.OnSendRequestToServer = async (serviceName, methodName, parameters) =>
            {
                if (IsForce)
                {
                    return ("", false);
                }
                (bool Success, string Result) = await ClientConnectionManager.TakeData(ClientConnectionManager.GetUrl(serviceName, methodName), parameters);
                if (Success)
                {
                    return (Result, true);
                }
                return ("", false);
            };

            clientProvider.OnGetResponseFromServer = (serviceName, methodName, parameters, result) =>
            {
                _ = ClientConnectionManager.SaveLocal(ClientConnectionManager.GetUrl(serviceName, methodName), parameters, result);
                return Task.CompletedTask;
            };

            clientProvider.ConnectAsyncAutoReconnect(ServiceAddress, (isConnected) =>
            {

            });
        }

        public static string ServiceAddress { get; set; } = "http://localhost:9341";//"http://localhost:9341"; "http://api.noorpod.ir";
        static TranslatorNoCacheHttpClient NoCacheHttpClient { get; set; } = new TranslatorNoCacheHttpClient();
        static TranslatorHttpClient CacheHttpClient { get; set; } = new TranslatorHttpClient();

        static IBookServiceAsync DuplexBookService { get; set; }
        static IChapterServiceAsync DuplexChapterService { get; set; }
        static IPageServiceAsync DuplexPageService { get; set; }
        static IHealthService DuplexHealthService { get; set; }
        static IAuthenticationServiceAsync DuplexAuthenticationService { get; set; }

        public static bool IsDuplexProtocol { get; set; }
        static bool IsForce { get; set; }

        public static IBookServiceAsync GetBookServiceHttp(bool isForce)
        {
            IsForce = isForce;
            if (IsDuplexProtocol)
                return DuplexBookService;
            if (isForce)
                return new TranslatorsServices.HttpServices.BookService(ServiceAddress, NoCacheHttpClient);
            else
                return new TranslatorsServices.HttpServices.BookService(ServiceAddress, CacheHttpClient);
        }

        public static IChapterServiceAsync GetChapterServiceHttp(bool isForce)
        {
            IsForce = isForce;
            if (IsDuplexProtocol)
                return DuplexChapterService;
            if (isForce)
                return new TranslatorsServices.HttpServices.ChapterService(ServiceAddress, NoCacheHttpClient);
            else
                return new TranslatorsServices.HttpServices.ChapterService(ServiceAddress, CacheHttpClient);
        }

        public static IPageServiceAsync GetPageServiceHttp(bool isForce)
        {
            IsForce = isForce;
            if (IsDuplexProtocol)
                return DuplexPageService;
            if (isForce)
                return new TranslatorsServices.HttpServices.PageService(ServiceAddress, NoCacheHttpClient);
            else
                return new TranslatorsServices.HttpServices.PageService(ServiceAddress, CacheHttpClient);
        }

        public static IHealthService GetHealthServiceHttp(bool isForce)
        {
            IsForce = isForce;
            if (IsDuplexProtocol)
                return DuplexHealthService;
            if (isForce)
                return new TranslatorsServices.HttpServices.HealthService(ServiceAddress, NoCacheHttpClient);
            else
                return new TranslatorsServices.HttpServices.HealthService(ServiceAddress, CacheHttpClient);
        }

        public static IAuthenticationServiceAsync GetAuthenticationServiceHttp(bool isForce)
        {
            IsForce = isForce;
            if (IsDuplexProtocol)
                return DuplexAuthenticationService;
            if (isForce)
                return new TranslatorsServices.HttpServices.AuthenticationService(ServiceAddress, NoCacheHttpClient);
            else
                return new TranslatorsServices.HttpServices.AuthenticationService(ServiceAddress, CacheHttpClient);
        }
    }
}