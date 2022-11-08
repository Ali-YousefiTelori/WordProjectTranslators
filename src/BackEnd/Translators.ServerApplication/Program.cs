using SignalGo.Server.ServiceManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
                serverProvider.RegisterServerService<BookService>();
                serverProvider.RegisterServerService<ChapterService>();
                serverProvider.RegisterServerService<PageService>();
                serverProvider.RegisterServerService<HealthService>(); 
                serverProvider.RegisterServerService<AuthenticationService>();
                serverProvider.RegisterServerService<ParagraphService>();
                serverProvider.RegisterServerService<ApplicationService>();
                serverProvider.RegisterServerService<UserReadingService>();
                
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