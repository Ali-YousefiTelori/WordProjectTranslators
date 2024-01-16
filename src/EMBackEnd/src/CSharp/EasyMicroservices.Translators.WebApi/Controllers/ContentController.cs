using EasyMicroservices.TranslatorsMicroservice.Contracts.Common;
using EasyMicroservices.TranslatorsMicroservice.Contracts.Requests;
using EasyMicroservices.TranslatorsMicroservice.Database.Entities;
using EasyMicroservices.Cores.AspCoreApi;
using EasyMicroservices.Cores.AspEntityFrameworkCoreApi.Interfaces;
using EasyMicroservices.Cores.Contracts.Requests;
using EasyMicroservices.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyMicroservices.TranslatorsMicroservice.WebApi.Controllers
{
    public class ContentController : SimpleQueryServiceController<ContentEntity, CreateContentRequestContract, UpdateContentRequestContract, ContentContract, long>
    {
        readonly IUnitOfWork unitOfWork;
        public ContentController(IUnitOfWork _unitOfWork) : base(_unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public override async Task<MessageContract<long>> Add(CreateContentRequestContract request, CancellationToken cancellationToken = default)
        {
            using var categorylogic = unitOfWork.GetLongContractLogic<CategoryEntity, CreateCategoryRequestContract, UpdateCategoryRequestContract, CategoryContract>();
            using var languageLogic = unitOfWork.GetLongContractLogic<LanguageEntity, LanguageContract>();
            var checkLanguageId = await languageLogic.GetById(new GetByIdRequestContract<long>() { Id = request.LanguageId });
            var checkCategoryId = await categorylogic.GetById(new GetByIdRequestContract<long>() { Id = request.CategoryId });
            if (checkLanguageId.IsSuccess && checkCategoryId.IsSuccess)
                return await base.Add(request, cancellationToken);
            return (FailedReasonType.Incorrect, "Language or Categoryid is incorrect");
        }

        public override async Task<MessageContract<ContentContract>> Update(UpdateContentRequestContract request, CancellationToken cancellationToken = default)
        {
            using var categorylogic = unitOfWork.GetLongContractLogic<CategoryEntity, CreateCategoryRequestContract, UpdateCategoryRequestContract, CategoryContract>();
            using var languageLogic = unitOfWork.GetLongContractLogic<LanguageEntity, LanguageContract>();
            var checkLanguageId = await languageLogic.GetById(new GetByIdRequestContract<long>() { Id = request.LanguageId });
            var checkCategoryId = await categorylogic.GetById(new GetByIdRequestContract<long>() { Id = request.CategoryId });
            if (checkLanguageId.IsSuccess && checkCategoryId.IsSuccess)
                return await base.Update(request, cancellationToken);
            return (FailedReasonType.Incorrect, "Language or Categoryid is incorrect");

        }

        [HttpPost]
        public async Task<MessageContract<ContentContract>> GetByLanguage(GetByLanguageRequestContract request)
        {
            using var categorylogic = unitOfWork.GetLongContractLogic<CategoryEntity, CreateCategoryRequestContract, UpdateCategoryRequestContract, CategoryContract>();
            var getCategoryResult = await categorylogic.GetByUniqueIdentity(request, Cores.DataTypes.GetUniqueIdentityType.All,
                query => query.Where(x => x.Key == request.Key)
                .Include(x => x.Translators)
                .ThenInclude(x => x.Language));
            if (!getCategoryResult)
                return getCategoryResult.ToContract<ContentContract>();
            var contentResult = getCategoryResult.Result.Translators
                .FirstOrDefault(x => x.Language.Name.Equals(request.Language, StringComparison.OrdinalIgnoreCase));
            if (contentResult == null)
                contentResult = getCategoryResult.Result.Translators.FirstOrDefault();
            if (contentResult == null)
                return (FailedReasonType.NotFound, $"Content {request.Key} by language {request.Language} not found!");

            return contentResult;
        }


        [HttpPost]
        public async Task<ListMessageContract<ContentContract>> GetAllByKey(GetAllByKeyRequestContract request)
        {
            using var categorylogic = unitOfWork.GetLongContractLogic<CategoryEntity, CreateCategoryRequestContract, UpdateCategoryRequestContract, CategoryContract>();
            var getCategoryResult = await categorylogic
                .GetByUniqueIdentity(request,
                    Cores.DataTypes.GetUniqueIdentityType.All,
                    query => query
                    .Where(x => x.Key == request.Key)
                    .Include(x => x.Translators)
                    .ThenInclude(x => x.Language));
            if (!getCategoryResult)
                return getCategoryResult.ToListContract<ContentContract>();

            return getCategoryResult.Result.Translators;
        }

        [HttpPost]
        public async Task<MessageContract<CategoryContract>> AddContentWithKey(AddContentWithKeyRequestContract request)
        {
            Console.WriteLine($"try add {request.Key}!");
            using var categorylogic = unitOfWork.GetLongContractLogic<CategoryEntity, CreateCategoryRequestContract, UpdateCategoryRequestContract, CategoryContract>();
            using var contentlogic = unitOfWork.GetLongContractLogic<ContentEntity, CreateContentRequestContract, UpdateContentRequestContract, ContentContract>();
            using var languageLogic = unitOfWork.GetLongContractLogic<LanguageEntity, LanguageContract>();

            var getCategoryResult = await categorylogic.GetByUniqueIdentity(request,
                Cores.DataTypes.GetUniqueIdentityType.All
                    , query => query.Where(x => x.Key == request.Key));
            if (getCategoryResult.IsSuccess)
                return (FailedReasonType.Duplicate, $"Category {request.Key} already exists.");

            var languages = await languageLogic.GetAll();
            var notFoundLanguages = request.LanguageData.Select(x => x.Language).Except(languages.Result.Select(o => o.Name));

            if (!notFoundLanguages.Any())
            {
                var addCategoryResult = await categorylogic.Add(new CreateCategoryRequestContract
                {
                    Key = request.Key,
                    UniqueIdentity = request.UniqueIdentity
                });

                if (!addCategoryResult.IsSuccess)
                    return addCategoryResult.ToContract<CategoryContract>();

                foreach (var item in request.LanguageData)
                {
                    var languageId = languages.Result.FirstOrDefault(o => o.Name == item.Language)?.Id;
                    if (!languageId.HasValue)
                        return (FailedReasonType.NotFound, $"Language {item.Language} not found!");

                    var addContentResult = await contentlogic.Add(new CreateContentRequestContract
                    {
                        CategoryId = addCategoryResult.Result,
                        LanguageId = languageId.Value,
                        Data = item.Data,
                        UniqueIdentity = request.UniqueIdentity
                    });
                }

                var addedCategoryResult = await categorylogic.GetById(new GetByIdRequestContract<long>
                {
                    Id = addCategoryResult.Result
                });

                return addedCategoryResult.Result;
            }
            return (FailedReasonType.Incorrect, $"This languages are not registered in the content server: {string.Join(", ", notFoundLanguages)}");
        }

        [HttpPost]
        public async Task<MessageContract> UpdateContentWithKey(AddContentWithKeyRequestContract request)
        {
            using var categorylogic = unitOfWork.GetLongContractLogic<CategoryEntity, CreateCategoryRequestContract, UpdateCategoryRequestContract, CategoryContract>();
            using var contentlogic = unitOfWork.GetLongContractLogic<ContentEntity, CreateContentRequestContract, UpdateContentRequestContract, ContentContract>();
            using var languageLogic = unitOfWork.GetLongContractLogic<LanguageEntity, LanguageContract>();
            var getCategoryResult = await categorylogic.GetByUniqueIdentity(request,
                 Cores.DataTypes.GetUniqueIdentityType.All,
                 query => query
                 .Where(x => x.Key == request.Key)
                 .Include(x => x.Translators)
                 .ThenInclude(x => x.Language));

            if (!getCategoryResult)
                return getCategoryResult;
            var Translators = getCategoryResult.Result.Translators;
            foreach (var content in Translators)
            {
                if (request.LanguageData.Any(o => o.Language == content.Language.Name))
                {
                    var response = await contentlogic.Update(new UpdateContentRequestContract
                    {
                        Id = content.Id,
                        CategoryId = content.CategoryId,
                        LanguageId = content.LanguageId,
                        UniqueIdentity = content.UniqueIdentity,
                        Data = request.LanguageData.FirstOrDefault(o => o.Language == content.Language.Name).Data
                    });

                    if (!response.IsSuccess)
                        return response.ToContract();
                }
            }

            foreach (var languageData in request.LanguageData)
            {
                if (!Translators.Any(o => o.Language.Name == languageData.Language))
                {
                    var language = await languageLogic.GetBy(o => o.Name == languageData.Language);

                    if (!language)
                        continue;

                    var response = await contentlogic.Add(new CreateContentRequestContract
                    {
                        CategoryId = getCategoryResult.Result.Id,
                        LanguageId = language.Result.Id,
                        UniqueIdentity = language.Result.UniqueIdentity,
                        Data = languageData.Data
                    });

                    if (!response)
                        return response.ToContract();
                }
            }

            return true;
        }

    }
}