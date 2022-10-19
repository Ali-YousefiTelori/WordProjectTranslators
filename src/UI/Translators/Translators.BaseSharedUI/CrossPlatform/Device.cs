using System;
using System.Collections.Generic;
using System.Text;

namespace Translators.BaseSharedUI.CrossPlatform
{
    public enum RuntimePlatform
    {

    }

    public class Device
    {
        public const string iOS = nameof(iOS);
        public const string Android = nameof(Android);
        public const string WPF = nameof(WPF);
        public static string RuntimePlatform { get; set; }  
    }
}
