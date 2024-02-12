﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Contracts.Common
{
    public class LogContract
    {
        public long Id { get; set; }
        public string LogTrace { get; set; }
        public int AppVersion { get; set; }
        public string Session { get; set; }
        public string DeviceDescription { get; set; }
    }
}
