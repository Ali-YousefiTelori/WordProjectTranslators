using SignalGo.Server.ServiceManager;
using Translators.Contracts.Common;
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
                await ConfigData.LoadAsync();
                ServerProvider serverProvider = new ServerProvider();
                serverProvider.RegisterServerService<BookService>();
                serverProvider.RegisterServerService<ChapterService>();
                serverProvider.RegisterServerService<PageService>();
                serverProvider.ErrorHandlingFunction = (ex, type, method, parameters, jsonParameter, client) =>
                {
                    return new MessageContract()
                    {
                        IsSuccess = false,
                        Error = new ErrorContract()
                        {
                            Message = ex.Message,
                            StackTrace = ex.StackTrace
                        }
                    };
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