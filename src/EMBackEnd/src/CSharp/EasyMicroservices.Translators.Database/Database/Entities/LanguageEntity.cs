using EasyMicroservices.TranslatorsMicroservice.Database.Schemas;
using EasyMicroservices.Cores.Interfaces;
using System.Collections.Generic;

namespace EasyMicroservices.TranslatorsMicroservice.Database.Entities
{
    public class LanguageEntity : LanguageSchema, IIdSchema<long>
    {
        public long Id { get; set; }
        public ICollection<ContentEntity> Translators { get; set; }
    }
}
