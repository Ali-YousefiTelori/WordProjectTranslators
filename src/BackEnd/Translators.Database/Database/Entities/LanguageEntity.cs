using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Database.Entities
{
    public class LanguageEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public List<LanguageValueEntity> LanguageValues { get; set; }
    }
}
