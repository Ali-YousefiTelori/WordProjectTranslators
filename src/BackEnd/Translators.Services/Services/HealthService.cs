using SignalGo.Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Database.Contexts;
using Translators.Database.Entities;
using Translators.Logics;
using Translators.Validations;

namespace Translators.Services
{
    [ServiceContract("Health", ServiceType.HttpService, InstanceType.SingleInstance)]
    [ServiceContract("Health", ServiceType.ServerService, InstanceType.SingleInstance)]
    public class HealthService
    {
        public async Task<MessageContract> AddLog(LogContract log)
        {
            var result = await new LogicBase<TranslatorContext, LogContract, LogEntity>().Add(log);
            return result;
        }
    }
}
