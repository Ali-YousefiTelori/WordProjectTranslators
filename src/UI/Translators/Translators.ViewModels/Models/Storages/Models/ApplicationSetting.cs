using System;
using System.Collections.Generic;
using System.Text;

namespace Translators.Models.Storages.Models
{
    public class ApplicationSetting
    {
        public int FontSize { get; set; }
        public bool UseDuplexProtocol { get; set; }
        public double PlaybackSpeedRato { get; set; } = 1;
        public bool HasAutoScrollInPlayback { get; set; }
        public bool ShowTransliteration { get; set; }
    }
}
