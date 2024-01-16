//using System.Threading.Tasks;
//using EasyMicroservices.Mapper.CompileTimeMapper.Interfaces;
//using EasyMicroservices.Mapper.Interfaces;
//using System.Linq;

//namespace CompileTimeMapper
//{
//    public class ContentEntity_ContentContract_Mapper : IMapper
//    {
//        readonly IMapperProvider _mapper;
//        public ContentEntity_ContentContract_Mapper(IMapperProvider mapper)
//        {
//            _mapper = mapper;
//        }
//        public global::EasyMicroservices.TranslatorsMicroservice.Database.Entities.ContentEntity Map(global::EasyMicroservices.TranslatorsMicroservice.Contracts.Common.ContentContract fromObject, string uniqueRecordId, string language, object[] parameters)
//        {
//            if (fromObject == default)
//                return default;
//            var mapped = new global::EasyMicroservices.TranslatorsMicroservice.Database.Entities.ContentEntity()
//            {
//                Id = fromObject.Id,
//                ParentId = fromObject.ParentId,
//                Name = fromObject.Name,
//                Text = fromObject.Text,
//                Email = fromObject.Email,
//                Website = fromObject.Website,
//                CreationDateTime = fromObject.CreationDateTime,
//                ModificationDateTime = fromObject.ModificationDateTime,
//                IsDeleted = fromObject.IsDeleted,
//                DeletedDateTime = fromObject.DeletedDateTime,
//                UniqueIdentity = fromObject.UniqueIdentity
//            };
//            return mapped;
//        }
//        public global::EasyMicroservices.TranslatorsMicroservice.Contracts.Common.ContentContract Map(global::EasyMicroservices.TranslatorsMicroservice.Database.Entities.ContentEntity fromObject, string uniqueRecordId, string language, object[] parameters)
//        {
//            if (fromObject == default)
//                return default;
//            var mapped = new global::EasyMicroservices.TranslatorsMicroservice.Contracts.Common.ContentContract()
//            {
//                Id = fromObject.Id,
//                ParentId = fromObject.ParentId,
//                Name = fromObject.Name,
//                Text = fromObject.Text,
//                Email = fromObject.Email,
//                Website = fromObject.Website,
//                CreationDateTime = fromObject.CreationDateTime,
//                ModificationDateTime = fromObject.ModificationDateTime,
//                IsDeleted = fromObject.IsDeleted,
//                DeletedDateTime = fromObject.DeletedDateTime,
//                UniqueIdentity = fromObject.UniqueIdentity
//            };
//            return mapped;
//        }
//        public async Task<global::EasyMicroservices.TranslatorsMicroservice.Database.Entities.ContentEntity> MapAsync(global::EasyMicroservices.TranslatorsMicroservice.Contracts.Common.ContentContract fromObject, string uniqueRecordId, string language, object[] parameters)
//        {
//            if (fromObject == default)
//                return default;
//            var mapped = new global::EasyMicroservices.TranslatorsMicroservice.Database.Entities.ContentEntity()
//            {
//                Id = fromObject.Id,
//                ParentId = fromObject.ParentId,
//                Name = fromObject.Name,
//                Text = fromObject.Text,
//                Email = fromObject.Email,
//                Website = fromObject.Website,
//                CreationDateTime = fromObject.CreationDateTime,
//                ModificationDateTime = fromObject.ModificationDateTime,
//                IsDeleted = fromObject.IsDeleted,
//                DeletedDateTime = fromObject.DeletedDateTime,
//                UniqueIdentity = fromObject.UniqueIdentity
//            };
//            return mapped;
//        }
//        public async Task<global::EasyMicroservices.TranslatorsMicroservice.Contracts.Common.ContentContract> MapAsync(global::EasyMicroservices.TranslatorsMicroservice.Database.Entities.ContentEntity fromObject, string uniqueRecordId, string language, object[] parameters)
//        {
//            if (fromObject == default)
//                return default;
//            var mapped = new global::EasyMicroservices.TranslatorsMicroservice.Contracts.Common.ContentContract()
//            {
//                Id = fromObject.Id,
//                ParentId = fromObject.ParentId,
//                Name = fromObject.Name,
//                Text = fromObject.Text,
//                Email = fromObject.Email,
//                Website = fromObject.Website,
//                CreationDateTime = fromObject.CreationDateTime,
//                ModificationDateTime = fromObject.ModificationDateTime,
//                IsDeleted = fromObject.IsDeleted,
//                DeletedDateTime = fromObject.DeletedDateTime,
//                UniqueIdentity = fromObject.UniqueIdentity

//            };
//            return mapped;
//        }
//        public object MapObject(object fromObject, string uniqueRecordId, string language, object[] parameters)
//        {
//            if (fromObject == default)
//                return default;
//            if (fromObject.GetType() == typeof(EasyMicroservices.TranslatorsMicroservice.Database.Entities.ContentEntity))
//                return Map((EasyMicroservices.TranslatorsMicroservice.Database.Entities.ContentEntity)fromObject, uniqueRecordId, language, parameters);
//            return Map((EasyMicroservices.TranslatorsMicroservice.Contracts.Common.ContentContract)fromObject, uniqueRecordId, language, parameters);
//        }
//        public async Task<object> MapObjectAsync(object fromObject, string uniqueRecordId, string language, object[] parameters)
//        {
//            if (fromObject == default)
//                return default;
//            if (fromObject.GetType() == typeof(EasyMicroservices.TranslatorsMicroservice.Database.Entities.ContentEntity))
//                return await MapAsync((EasyMicroservices.TranslatorsMicroservice.Database.Entities.ContentEntity)fromObject, uniqueRecordId, language, parameters);
//            return await MapAsync((EasyMicroservices.TranslatorsMicroservice.Contracts.Common.ContentContract)fromObject, uniqueRecordId, language, parameters);
//        }
//    }
//}