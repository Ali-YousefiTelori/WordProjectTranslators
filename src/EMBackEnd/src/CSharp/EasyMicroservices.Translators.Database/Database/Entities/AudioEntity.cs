using Translators.Schemas;

namespace Translators.Database.Entities
{
    public class AudioEntity : AudioSchema
    {
        public byte[] Data { get; set; }
        public PageEntity Page { get; set; }
        public LanguageEntity Language { get; set; }
        public ParagraphEntity Paragraph { get; set; }
        public AudioReaderEntity AudioReader { get; set; }
        public TranslatorEntity Translator { get; set; }
    }
}
