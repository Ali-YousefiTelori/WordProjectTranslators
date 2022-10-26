using System;
using System.Collections.Generic;
using System.Text;
using Translators.Models.Interfaces;

namespace Translators.Helpers
{
    public class AudioPlayerBaseHelper
    {
        public static IAudioPlayer CurrentBase { get; set; }
    }
}
