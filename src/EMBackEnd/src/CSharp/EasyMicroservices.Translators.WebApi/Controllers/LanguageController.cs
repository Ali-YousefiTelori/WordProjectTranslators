using EasyMicroservices.TranslatorsMicroservice.Contracts.Common;
using EasyMicroservices.TranslatorsMicroservice.Contracts.Requests;
using EasyMicroservices.TranslatorsMicroservice.Database.Entities;
using EasyMicroservices.Cores.AspCoreApi;
using EasyMicroservices.Cores.AspEntityFrameworkCoreApi.Interfaces;
using EasyMicroservices.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace EasyMicroservices.TranslatorsMicroservice.WebApi.Controllers
{
    public class LanguageController : SimpleQueryServiceController<LanguageEntity, CreateLanguageRequestContract, UpdateLanguageRequestContract, LanguageContract, long>
    {
        private readonly IUnitOfWork _unitOfWork;

        public LanguageController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<MessageContract> HasLanguage(HasLanguageRequestContract hasLanguageRequest)
        {
            return await _unitOfWork.GetLongContractLogic<LanguageEntity, LanguageContract>()
                .GetBy(x => x.Name == hasLanguageRequest.Language);
        }
    }
}
