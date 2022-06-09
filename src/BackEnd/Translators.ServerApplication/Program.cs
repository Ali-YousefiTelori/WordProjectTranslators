using SignalGo.Server.ServiceManager;
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