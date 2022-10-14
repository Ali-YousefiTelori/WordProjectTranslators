using SignalGo.Client;
using System;
using System.Threading.Tasks;
using Translators.Models;
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
            DuplexParagraphService = clientProvider.RegisterServerService<TranslatorsServices.ServerServices.ParagraphService>(clientProvider);
            DuplexApplicationService = clientProvider.RegisterServerService<TranslatorsServices.ServerServices.ApplicationService>(clientProvider);

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

        public static string ServiceAddress { get; set; } = "http://192.168.55.22:9341";//"http://localhost:9341"; "http://api.noorpod.ir"; "http://192.168.55.22:9341";
        static TranslatorNoCacheHttpClient NoCacheHttpClient { get; set; } = new TranslatorNoCacheHttpClient();
        static TranslatorHttpClient CacheHttpClient { get; set; } = new TranslatorHttpClient();

        static IBookServiceAsync DuplexBookService { get; set; }
        static IChapterServiceAsync DuplexChapterService { get; set; }
        static IPageServiceAsync DuplexPageService { get; set; }
        static IHealthService DuplexHealthService { get; set; }
        static IAuthenticationServiceAsync DuplexAuthenticationService { get; set; }
        static IApplicationServiceAsync DuplexApplicationService { get; set; }
        static IParagraphServiceAsync DuplexParagraphService { get; set; }

        public static bool IsAdmin { get; set; }
        public static bool IsDuplexProtocol { get; set; }
        static bool IsForce { get; set; }
        public static ParagraphBaseModel[] ParagraphsForLink { get; set; }
        public static IBookServiceAsync GetBookService(bool isForce)
        {
            IsForce = isForce;
            if (IsDuplexProtocol)
                return DuplexBookService;
            if (isForce)
                return new TranslatorsServices.HttpServices.BookService(ServiceAddress, NoCacheHttpClient);
            else
                return new TranslatorsServices.HttpServices.BookService(ServiceAddress, CacheHttpClient);
        }

        public static IChapterServiceAsync GetChapterService(bool isForce)
        {
            IsForce = isForce;
            if (IsDuplexProtocol)
                return DuplexChapterService;
            if (isForce)
                return new TranslatorsServices.HttpServices.ChapterService(ServiceAddress, NoCacheHttpClient);
            else
                return new TranslatorsServices.HttpServices.ChapterService(ServiceAddress, CacheHttpClient);
        }

        public static IPageServiceAsync GetPageService(bool isForce)
        {
            IsForce = isForce;
            if (IsDuplexProtocol)
                return DuplexPageService;
            if (isForce)
                return new TranslatorsServices.HttpServices.PageService(ServiceAddress, NoCacheHttpClient);
            else
                return new TranslatorsServices.HttpServices.PageService(ServiceAddress, CacheHttpClient);
        }

        public static IHealthService GetHealthService(bool isForce)
        {
            IsForce = isForce;
            if (IsDuplexProtocol)
                return DuplexHealthService;
            if (isForce)
                return new TranslatorsServices.HttpServices.HealthService(ServiceAddress, NoCacheHttpClient);
            else
                return new TranslatorsServices.HttpServices.HealthService(ServiceAddress, CacheHttpClient);
        }

        public static IAuthenticationServiceAsync GetAuthenticationService(bool isForce)
        {
            IsForce = isForce;
            if (IsDuplexProtocol)
                return DuplexAuthenticationService;
            if (isForce)
                return new TranslatorsServices.HttpServices.AuthenticationService(ServiceAddress, NoCacheHttpClient);
            else
                return new TranslatorsServices.HttpServices.AuthenticationService(ServiceAddress, CacheHttpClient);
        }

        public static IApplicationServiceAsync GetApplicationService(bool isForce)
        {
            IsForce = isForce;
            if (IsDuplexProtocol)
                return DuplexApplicationService;
            if (isForce)
                return new TranslatorsServices.HttpServices.ApplicationService(ServiceAddress, NoCacheHttpClient);
            else
                return new TranslatorsServices.HttpServices.ApplicationService(ServiceAddress, CacheHttpClient);
        }

        public static IParagraphServiceAsync GetParagraphService(bool isForce)
        {
            IsForce = isForce;
            if (IsDuplexProtocol)
                return DuplexParagraphService;
            if (isForce)
                return new TranslatorsServices.HttpServices.ParagraphService(ServiceAddress, NoCacheHttpClient);
            else
                return new TranslatorsServices.HttpServices.ParagraphService(ServiceAddress, CacheHttpClient);
        }

        public static Func<string> GetVersion { get; set; }
        public static Func<string> GetCurrentVersionNumber { get; set; }
        public static void LogException(string error)
        {
            try
            {
                int.TryParse(GetCurrentVersionNumber(), out int version);
                GetHealthService(true).AddLog(new Contracts.Common.LogContract()
                {
                    LogTrace = error,
                    DeviceDescription = GetVersion(),
                    AppVersion = version,
                    Session = TranslatorService.Session
                });
            }
            catch
            {

            }
        }
    }
}