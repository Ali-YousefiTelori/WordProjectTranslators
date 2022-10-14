using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Translators.Models.Interfaces;
using Translators.ServiceManagers;

namespace Translators.Helpers
{
    public class ApplicationHelper
    {
        public static IApplicationManager Current { get; set; }

        public static Task<long> GetBuildNumber()
        {
            if (long.TryParse(TranslatorService.GetCurrentVersionNumber(), out long number))
                return Task.FromResult(number);
            return Task.FromResult(0L);
        }
    }
}
