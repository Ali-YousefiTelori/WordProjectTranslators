using TranslatorApp.GeneratedServices;
using Translators.Models;
using Translators.Models.Storages;

namespace Translators.ServiceManagers
{
    public static class TranslatorService
    {
        public static void Initialize()
        {
            //ClientProvider clientProvider = new ClientProvider();
            //DuplexBookService = clientProvider.RegisterServerService<TranslatorsServices.ServerServices.BookService>(clientProvider);
            //DuplexChapterService = clientProvider.RegisterServerService<TranslatorsServices.ServerServices.ChapterService>(clientProvider);
            //OldDuplexPageService = clientProvider.RegisterServerService<TranslatorsServices.ServerServices.ObsoletePageService>(clientProvider);
            //DuplexPageService = clientProvider.RegisterServerService<TranslatorsServices.ServerServices.PageService>(clientProvider);
            //DuplexHealthService = clientProvider.RegisterServerService<TranslatorsServices.ServerServices.HealthService>(clientProvider);
            //DuplexAuthenticationService = clientProvider.RegisterServerService<TranslatorsServices.ServerServices.AuthenticationService>(clientProvider);
            //OldDuplexAuthenticationService = clientProvider.RegisterServerService<TranslatorsServices.ServerServices.ObsoleteAuthenticationService>(clientProvider);
            //DuplexParagraphService = clientProvider.RegisterServerService<TranslatorsServices.ServerServices.ParagraphService>(clientProvider);
            //DuplexApplicationService = clientProvider.RegisterServerService<TranslatorsServices.ServerServices.ApplicationService>(clientProvider);

            //clientProvider.OnSendRequestToServer = async (serviceName, methodName, parameters) =>
            //{
            //    if (IsForce)
            //    {
            //        return ("", false);
            //    }
            //    (bool Success, string Result) = await ClientConnectionManager.TakeData(ClientConnectionManager.GetUrl(serviceName, methodName), parameters);
            //    if (Success)
            //    {
            //        return (Result, true);
            //    }
            //    return ("", false);
            //};

            //clientProvider.OnGetResponseFromServer = async (serviceName, methodName, parameters, result) =>
            //{
            //    _ = ClientConnectionManager.SaveLocal(ClientConnectionManager.GetUrl(serviceName, methodName), parameters, result);
            //};

            //clientProvider.ConnectAsyncAutoReconnect(ServiceAddress, (isConnected) =>
            //{

            //});
        }

        public static string ServiceAddress { get; set; } = "https://api.noorpod.ir";//"http://localhost:9341"; "http://api.noorpod.ir"; "http://192.168.55.22:9341";
        static TranslatorNoCacheHttpClient NoCacheHttpClient { get; set; } = new TranslatorNoCacheHttpClient();
        static TranslatorHttpClient CacheHttpClient { get; set; } = new TranslatorHttpClient();

        public static bool IsAdmin { get; set; }
        static bool IsForce { get; set; }
        public static ParagraphBaseModel[] ParagraphsForLink { get; set; }
        public static BookClient GetBookService(bool isForce)
        {
            IsForce = isForce;
            if (isForce)
                return new BookClient(ServiceAddress, NoCacheHttpClient);
            else
                return new BookClient(ServiceAddress, CacheHttpClient);
        }

        public static ChapterClient GetChapterService(bool isForce)
        {
            IsForce = isForce;
            if (isForce)
                return new ChapterClient(ServiceAddress, NoCacheHttpClient);
            else
                return new ChapterClient(ServiceAddress, CacheHttpClient);
        }

        public static ObsoletePageClient GetOldPageService(bool isForce)
        {
            IsForce = isForce;
            if (isForce)
                return new ObsoletePageClient(ServiceAddress, NoCacheHttpClient);
            else
                return new ObsoletePageClient(ServiceAddress, CacheHttpClient);
        }

        public static PageClient GetPageService(bool isForce)
        {
            IsForce = isForce;
            if (isForce)
                return new PageClient(ServiceAddress, NoCacheHttpClient);
            else
                return new PageClient(ServiceAddress, CacheHttpClient);
        }

        public static HealthClient GetHealthService(bool isForce)
        {
            IsForce = isForce;
            if (isForce)
                return new HealthClient(ServiceAddress, NoCacheHttpClient);
            else
                return new HealthClient(ServiceAddress, CacheHttpClient);
        }

        public static StorageClient GetStorageService(bool isForce)
        {
            IsForce = isForce;
            if (isForce)
                return new StorageClient(ServiceAddress, NoCacheHttpClient);
            else
                return new StorageClient(ServiceAddress, CacheHttpClient);
        }


        //public static ObsoleteAuthenticationClient GetOldAuthenticationService(bool isForce)
        //{
        //    IsForce = isForce;
        //    if (IsDuplexProtocol)
        //        return OldDuplexAuthenticationService;
        //    if (isForce)
        //        return new TranslatorsServices.HttpServices.ObsoleteAuthenticationService(ServiceAddress, NoCacheHttpClient);
        //    else
        //        return new TranslatorsServices.HttpServices.ObsoleteAuthenticationService(ServiceAddress, CacheHttpClient);
        //}

        //public static IAuthenticationServiceAsync GetAuthenticationService(bool isForce)
        //{
        //    IsForce = isForce;
        //    if (IsDuplexProtocol)
        //        return DuplexAuthenticationService;
        //    if (isForce)
        //        return new TranslatorsServices.HttpServices.AuthenticationService(ServiceAddress, NoCacheHttpClient);
        //    else
        //        return new TranslatorsServices.HttpServices.AuthenticationService(ServiceAddress, CacheHttpClient);
        //}

        public static ApplicationClient GetApplicationService(bool isForce)
        {
            IsForce = isForce;
            if (isForce)
                return new ApplicationClient(ServiceAddress, NoCacheHttpClient);
            else
                return new ApplicationClient(ServiceAddress, CacheHttpClient);
        }

        public static ParagraphClient GetParagraphService(bool isForce)
        {
            IsForce = isForce;
            if (isForce)
                return new ParagraphClient(ServiceAddress, NoCacheHttpClient);
            else
                return new ParagraphClient(ServiceAddress, CacheHttpClient);
        }

        public static UserReadingClient GetUserReadingService(bool isForce)
        {
            IsForce = isForce;
            if (isForce)
                return new UserReadingClient(ServiceAddress, NoCacheHttpClient);
            else
                return new UserReadingClient(ServiceAddress, CacheHttpClient);
        }

        public static Func<string> GetVersion { get; set; }
        public static Func<string> GetCurrentVersion { get; set; }
        public static Func<string> GetCurrentBuildNumber { get; set; }
        public static void LogException(string error)
        {
            try
            {
                int.TryParse(GetCurrentBuildNumber(), out int version);
                GetHealthService(true).AddLogAsync(new LogContract
                {
                    LogTrace = error,
                    DeviceDescription = GetVersion(),
                    AppVersion = version,
                    Session = ApplicationProfileData.Current?.Value?.Session?.ToString()
                });
            }
            catch
            {

            }
        }
    }
}