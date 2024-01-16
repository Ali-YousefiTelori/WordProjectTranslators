using EasyMicroservices.TranslatorsMicroservice.Database.Schemas;
using EasyMicroservices.Cores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.TranslatorsMicroservice.Database.Entities
{
    public class CategoryEntity : CategorySchema, IIdSchema<long>
    {
        public long Id { get; set; }
        public ICollection<ContentEntity> Translators { get; set; }
    }
}
