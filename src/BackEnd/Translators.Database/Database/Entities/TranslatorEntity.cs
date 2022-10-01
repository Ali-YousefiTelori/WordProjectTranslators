using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Database.Entities
{
    /// <summary>
    /// translator of book
    /// </summary>
    public class TranslatorEntity
    {
        public long Id { get; set; }
        /// <summary>
        /// نام مترجم
        /// </summary>
        public List<ValueEntity> Names { get; set; }
        /// <summary>
        /// چیزهایی که ترجمه کرده
        /// </summary>
        public List<ValueEntity> Values { get; set; }
        public List<AudioEntity> Audios { get; set; }
    }
}
