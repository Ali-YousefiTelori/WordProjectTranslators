using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Database.Entities.Authentications
{
    public class UserEntity
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        /// <summary>
        /// sms confirmed and register successful
        /// </summary>
        public bool IsConfirmed { get; set; }
        public Guid? UserSession { get; set; }

        public List<UserPermissionEntity> UserPermissions { get; set; }
    }
}
