using Translators.Schemas.Bases;

namespace Translators.Database.Entities
{
    /// <summary>
    /// translator of book
    /// </summary>
    public class TranslatorEntity : TranslatorSchemaBase
    {
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
