using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Database.Entities
{
    public class AudioReaderEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<AudioEntity> Audios { get; set; }
    }
}
