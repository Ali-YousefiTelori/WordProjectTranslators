using EasyMicroservices.ServiceContracts;
using EasyMicroservices.TranslatorsMicroservice;
using Microsoft.AspNetCore.Mvc;
using Translators.Contracts.Common;
using Translators.Database.Contexts;
using Translators.Database.Entities;
using Translators.Logics;

namespace Translators.Services
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HealthController : ControllerBase
    {
        AppUnitOfWork _appUnitOfWork;
        public HealthController(AppUnitOfWork appUnitOfWork)
        {
            _appUnitOfWork = appUnitOfWork;
        }

        [HttpPost]
        public async Task<MessageContract> AddLog(LogContract log)
        {
            using var context = _appUnitOfWork.GetContext();
            var result = await new LogicBase<LogContract, LogEntity>(context).Add(log);
            return result;
        }
    }
}
