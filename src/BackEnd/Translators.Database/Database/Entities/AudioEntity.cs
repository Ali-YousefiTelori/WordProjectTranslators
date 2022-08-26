using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Database.Entities
{
    public class AudioEntity
    {
        public long Id { get; set; }
        public string FileName { get; set; }
        public byte[] Data { get; set; }

        public long? PageId { get; set; }

        public PageEntity Page { get; set; }
    }
}
