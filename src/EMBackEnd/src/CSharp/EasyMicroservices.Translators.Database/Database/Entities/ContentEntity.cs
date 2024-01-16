using EasyMicroservices.TranslatorsMicroservice.Database.Schemas;
using EasyMicroservices.Cores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.TranslatorsMicroservice.Database.Entities
{
    public class ContentEntity : Translatorschema, IIdSchema<long>
    {
        public long Id { get; set; }
        public long LanguageId { get; set; }
        public LanguageEntity Language { get; set; }
        public long CategoryId { get; set; }
        public CategoryEntity Category { get; set; }
    }
}
