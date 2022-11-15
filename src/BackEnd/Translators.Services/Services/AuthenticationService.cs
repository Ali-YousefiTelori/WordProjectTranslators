using SignalGo.Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translators.Contracts.Common.Authentications;
using Translators.Contracts.Common;
using Translators.Contracts.Requests;

namespace Translators.Services
{
    [ServiceContract("authentication/v1", ServiceType.HttpService, InstanceType.SingleInstance)]
    [ServiceContract("authentication/v1", ServiceType.ServerService, InstanceType.SingleInstance)]
    public class AuthenticationService
    {
        public async Task<MessageContract> AppInit(AppInitRequestContract appInitRequest)
        {
            return true;
        }
    }
}
