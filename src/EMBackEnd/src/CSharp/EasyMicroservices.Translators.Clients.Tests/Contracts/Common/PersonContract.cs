using EasyMicroservices.TranslatorsMicroservice.Clients.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.TranslatorsMicroservice.Clients.Tests.Contracts.Common
{
    public class PersonContract
    {
        [ContentLanguage]
        public string Title { get; set; }
        public PostContract Post { get; set; }
        public List<PostContract> Posts { get; set; }
        public string UniqueIdentity { get; set; }
    }
}
