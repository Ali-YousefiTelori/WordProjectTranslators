using System.Collections.Generic;
using Translators.Schemas;

namespace Translators.Database.Entities
{
    public class LanguageEntity : LanguageSchema
    {
        public List<ValueEntity> Values { get; set; }
        public List<AudioEntity> Audios { get; set; }
    }
}
