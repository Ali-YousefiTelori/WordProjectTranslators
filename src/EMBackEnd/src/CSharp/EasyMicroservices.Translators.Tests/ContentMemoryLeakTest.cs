using EasyMicroservices.TranslatorsMicroservice.Contracts.Common;
using EasyMicroservices.TranslatorsMicroservice.Contracts.Requests;
using EasyMicroservices.TranslatorsMicroservice.Database.Contexts;
using EasyMicroservices.TranslatorsMicroservice.Database.Entities;
using EasyMicroservices.TranslatorsMicroservice.WebApi.Controllers;
using EasyMicroservices.Cores.AspEntityFrameworkCoreApi;
using EasyMicroservices.Cores.AspEntityFrameworkCoreApi.Interfaces;
using EasyMicroservices.Cores.Relational.EntityFrameworkCore;
using EasyMicroservices.Cores.Relational.EntityFrameworkCore.Intrerfaces;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace EasyMicroservices.TranslatorsMicroservice.Tests
{
    public class ContentMemoryLeakTest
    {
        static bool IsStarted = false;

        static async Task StartServer()
        {
            if (IsStarted)
                return;
            IsStarted = true;
            _ = Task.Run(async () =>
            {
                await EasyMicroservices.TranslatorsMicroservice.WebApi.Program.Run(null, (s) =>
                {
                    s.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(CategoryController).Assembly));
                });
            });
            await Task.Delay(2000);
        }

        //[Fact]
        //public async Task CheckMemoryLeakAddContentTest()
        //{
        //    await StartServer();
        //    Translators.GeneratedServices.CategoryClient client = new Translators.GeneratedServices.CategoryClient("http://localhost:2003", new System.Net.Http.HttpClient());
        //    var categoryResult = await client.AddAsync(new Translators.GeneratedServices.CreateCategoryRequestContract()
        //    {
        //        Key = $"1-1-Title{Guid.NewGuid()}",
        //        UniqueIdentity = "1-2"
        //    });
        //    Assert.True(categoryResult.IsSuccess);
        //    Translators.GeneratedServices.ContentClient contentClient = new Translators.GeneratedServices.ContentClient("http://localhost:2003", new System.Net.Http.HttpClient());
        //    for (int i = 0; i < 5000; i++)
        //    {
        //        var contentResult = await contentClient.AddAsync(new Translators.GeneratedServices.CreateContentRequestContract()
        //        {
        //            CategoryId = categoryResult.Result,
        //            UniqueIdentity = "1-2",
        //            Data = i.ToString(),
        //            LanguageId = 1
        //        });
        //        Assert.True(contentResult.IsSuccess);
        //    }

        //    while (true)
        //    {
        //        GC.Collect();
        //        await Task.Delay(1000);
        //    }
        //}

        //[Fact]
        //public async Task MemoryLeackTest()
        //{
        //    HostApplicationBuilder builder = Host.CreateApplicationBuilder(null);
        //    UnitOfWork.DefaultUniqueIdentity = "1-1";
        //    UnitOfWork.MicroserviceId = 10;
        //    builder.Services.AddTransient<IEntityFrameworkCoreDatabaseBuilder, DatabaseBuilder>();
        //    builder.Services.AddTransient<RelationalCoreContext, ContentContext>();
        //    builder.Services.AddTransient<ContentContext>();
        //    //builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

        //    using IHost host = builder.Build();

        //    for (int i = 0; i < 5000; i++)
        //    {
        //        using var scope = host.Services.CreateScope();
        //        using var uow = scope.ServiceProvider.GetService<RelationalCoreContext>();
        //        //using var logic = uow.GetLongContractLogic<ContentEntity, CreateContentRequestContract, UpdateContentRequestContract, ContentContract>();
        //    }
        //}
    }
}