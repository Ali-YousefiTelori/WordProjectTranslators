using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Contracts.Responses
{
    public class AppVersionResponseContract
    {
        /// <summary>
        /// نسخه ی موجود اپ
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// حتما باید به نسخه ی جدید بروزرسانی شود
        /// </summary>
        public int ForceUpdateNumber { get; set; }
        /// <summary>
        /// قبل از این نسخه باید کش کلاینت باید پاک شود
        /// </summary>
        public int CleanCacheTempNumber { get; set; }
    }
}
