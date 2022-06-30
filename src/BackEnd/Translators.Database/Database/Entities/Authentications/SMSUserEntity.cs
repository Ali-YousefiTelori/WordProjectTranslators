using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Database.Entities.Authentications
{
    public class SMSUserEntity
    {
        public long Id { get; set; }
        public string ApiSession { get; set; }
        public string PatternKey { get; set; }
    }
}
