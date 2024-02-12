using EasyMicroservices.Cores.AspEntityFrameworkCoreApi;
using EasyMicroservices.Cores.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using Translators.Database.Contexts;
using Translators.Logics;
using Translators.Models;

namespace EasyMicroservices.TranslatorsMicroservice;

public class AppUnitOfWork : UnitOfWork
{
    public AppUnitOfWork(IServiceProvider service) : base(service)
    {
    }

    public TranslatorContext GetContext()
    {
        return ServiceProvider.GetService<TranslatorContext>();
    }

    public CacheLogic GetCacheLogic()
    {
        return ServiceProvider.GetService<CacheLogic>();
    }

    public StorageLogic GetStorageLogic()
    {
        return ServiceProvider.GetService<StorageLogic>();
    }
    public CurrentUser GetCurrentUser()
    {
        var context = ServiceProvider.GetService<IHttpContextAccessor>()?.HttpContext;
        return new CurrentUser()
        {
            UserId = long.Parse(context.User.FindFirstValue(nameof(CurrentUser.UserId))),
        };
    }
}
