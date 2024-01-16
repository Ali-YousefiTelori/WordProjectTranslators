using EasyMicroservices.TranslatorsMicroservice.Clients.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.TranslatorsMicroservice.Clients.Tests.Contracts.Common
{
    public class PostContract
    {
        public long Id { get; set; }
        [ContentLanguage]
        public string Title { get; set; }
        [ContentLanguage]
        public string Content { get; set; }
        public PersonContract Person { get; set; }

        public string UniqueIdentity { get; set; }
    }
}
