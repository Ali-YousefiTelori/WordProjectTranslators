using EasyMicroservices.ServiceContracts;
using EasyMicroservices.TranslatorsMicroservice;
using Microsoft.AspNetCore.Mvc;
using Translators.Contracts.Requests.Pages;
using Translators.Contracts.Responses.Pages;
using Translators.Logics;

namespace Translators.Services
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PageController
    {
        AppUnitOfWork _appUnitOfWork;
        public PageController(AppUnitOfWork appUnitOfWork)
        {
            _appUnitOfWork = appUnitOfWork;
        }
        [HttpPost]
        public async Task<MessageContract<PageResponseContract>> GetPage(PageRequest request)
        {
            return await new PageLogic(_appUnitOfWork).GetSimplePage(request.PageNumber, request.BookId);
        }
    }
}
