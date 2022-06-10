using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Patches
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class TranslatorQuranAyat
    {
        public List<TranslatorQuranAyat> Ayat { get; set; }
        public string Number { get; set; }
        public TranslatorQuranAyeha Ayeha { get; set; }
    }

    public class TranslatorQuranAyeha
    {
        public TranslatorQuranLanguageText LanguageText { get; set; }
    }

    public class TranslatorQuranBooks
    {
        public TranslatorQuranKetab Ketab { get; set; }
    }

    public class TranslatorQuranKetab
    {
        public string Name { get; set; }
        public TranslatorQuranSureha Sureha { get; set; }
    }

    public class TranslatorQuranLanguageText
    {
        public string Language { get; set; }
        public string TeXt { get; set; }
    }

    public class TranslatorQuranMainBook
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public string LanguageID { get; set; }
        public TranslatorQuranBooks Books { get; set; }
    }

    public class TranslatorQuranRoot
    {
        public TranslatorQuranMainBook MainBook { get; set; }
    }

    public class TranslatorQuranSure
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public List<TranslatorQuranAyat> Ayat { get; set; }
    }

    public class TranslatorQuranSureha
    {
        public List<TranslatorQuranSure> Sure { get; set; }
    }
}
