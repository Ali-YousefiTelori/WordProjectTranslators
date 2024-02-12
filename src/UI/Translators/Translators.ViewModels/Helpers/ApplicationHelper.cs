using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TranslatorApp.GeneratedServices;
using Translators.Models.Interfaces;
using Translators.ServiceManagers;

namespace Translators.Helpers
{
    public class ApplicationHelper
    {
        public static ApplicationType ApplicationType { get; set; } = ApplicationType.None;
        public static IApplicationManager Current { get; set; }

        public static Task<long> GetBuildNumber()
        {
            return Current.GetBuildNumber();
        }
    }
}
