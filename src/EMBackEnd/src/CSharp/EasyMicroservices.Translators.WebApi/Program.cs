using EasyMicroservices.Cores.AspEntityFrameworkCoreApi;
using EasyMicroservices.Cores.Relational.EntityFrameworkCore.Intrerfaces;
using Translators.Database.Contexts;
using Translators.Logics;
using Translators.Shared.FileVersionControl;

namespace EasyMicroservices.TranslatorsMicroservice.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var app = CreateBuilder(args);
            var build = await app.BuildWithUseCors<TranslatorContext>(null, true);
            build.MapControllers();
            await SchemaVersionControl.Current.LoadAll();
            build.Run();
        }

        static WebApplicationBuilder CreateBuilder(string[] args)
        {
            var app = StartUpExtensions.Create<TranslatorContext>(args);
            app.Services.Builder<TranslatorContext>("Translators");
            app.Services.AddTransient((serviceProvider) => new UnitOfWork(serviceProvider));
            app.Services.AddTransient(serviceProvider => new TranslatorContext(serviceProvider.GetService<IEntityFrameworkCoreDatabaseBuilder>()));
            app.Services.AddTransient<IEntityFrameworkCoreDatabaseBuilder, DatabaseBuilder>();
            app.Services.AddTransient<AppUnitOfWork>();
            app.Services.AddTransient<StorageLogic>();
            app.Services.AddTransient<PageLogic>();
            app.Services.AddSingleton<CacheLogic>();
            SchemaVersionControl.Initialize();
            return app;
        }

        public static async Task Run(string[] args, Action<IServiceCollection> use)
        {
            var app = CreateBuilder(args);
            use?.Invoke(app.Services);
            var build = await app.Build<TranslatorContext>();
            build.MapControllers();
            build.Run();
        }
    }
}