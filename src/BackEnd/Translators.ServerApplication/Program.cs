using Newtonsoft.Json;
using SignalGo.Server.ServiceManager;
using SignalGo.Shared.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Translators.Attributes;
using Translators.Contracts.Common;
using Translators.Logics;
using Translators.Models;
using Translators.Services;

namespace Translators.ServerApplication
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting...");
                await ConfigData.LoadAsync();
                await CacheLogic.Initialize();

                ServerProvider serverProvider = new ServerProvider();
                serverProvider.ProviderSetting.IsEnabledDataExchanger = false;
                serverProvider.ProviderSetting.IsEnabledReferenceResolver = false;
                serverProvider.ProviderSetting.IsEnabledReferenceResolverForArray = false;
                serverProvider.RegisterServerService<BookService>();
                serverProvider.RegisterServerService<ChapterService>();
                serverProvider.RegisterServerService<ObsoletePageService>();
                serverProvider.RegisterServerService<PageService>();
                serverProvider.RegisterServerService<HealthService>();
                serverProvider.RegisterServerService<AuthenticationService>();
                serverProvider.RegisterServerService<ObsoleteAuthenticationService>();
                serverProvider.RegisterServerService<ParagraphService>();
                serverProvider.RegisterServerService<ApplicationService>();
                serverProvider.RegisterServerService<UserReadingService>();
                serverProvider.RegisterServerService<StorageService>();
                AutoLogger logger = new AutoLogger() { FileName = "Failed Operations.log" };

                serverProvider.ErrorHandlingFunction = (ex, type, method, parameters, jsonParameter, client) =>
                {
                    return new MessageContract()
                    {
                        IsSuccess = false,
                        Error = new ErrorContract()
                        {
                            FailedReasonType = FailedReasonType.InternalError,
                            Message = ex.Message,
                            StackTrace = ex.StackTrace
                        }
                    };
                };

                serverProvider.OnSendResponseToClientFunction = (request, response, type, method, client, guid) =>
                {
                    if (response is MessageContract messageContract && !messageContract)
                    {
                        logger.LogText($"Operation failed: {type?.FullName} {method?.Name} {JsonConvert.SerializeObject(request)} {response}");
                    }
                    return response;
                };
                serverProvider.ValidationResultHandlingFunction = (validations, obj, method) =>
                {
                    var result = new MessageContract()
                    {
                        IsSuccess = false,
                        Error = new ErrorContract()
                        {
                            FailedReasonType = FailedReasonType.ValidationsError,
                            Message = "You have validations error!",
                            StackTrace = Environment.StackTrace,
                        }
                    };
                    foreach (var item in validations)
                    {
                        result.Error.Validations.Add(item);
                    }
                    return result;
                };
                serverProvider.Start("http://localhost:9341");
                Console.WriteLine("Started on http://localhost:9341.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.ReadLine();
        }
    }
}