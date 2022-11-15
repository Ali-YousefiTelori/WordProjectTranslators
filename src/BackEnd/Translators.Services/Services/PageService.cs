using SignalGo.Shared.DataTypes;
using Translators.Attributes;
using Translators.Contracts.Common;
using Translators.Contracts.Requests.Pages;
using Translators.Contracts.Responses.Pages;
using Translators.Logics;
using Translators.Validations;

namespace Translators.Services
{
    [ServiceContract("page/v1", ServiceType.HttpService, InstanceType.SingleInstance)]
    [ServiceContract("page/v1", ServiceType.ServerService, InstanceType.SingleInstance)]
    public class PageService
    {
        [FastJsonSerialization]
        public async Task<MessageContract<PageResponseContract>> GetPage([NumberValidation] PageRequest request)
        {
            return await new PageLogic().GetSimplePage(request.PageNumber, request.BookId);
        }
    }
}
