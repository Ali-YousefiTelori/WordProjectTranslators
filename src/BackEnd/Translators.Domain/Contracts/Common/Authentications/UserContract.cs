﻿using SignalGo.Shared.DataTypes;
using Translators.Contracts.Common.DataTypes;

namespace Translators.Contracts.Common.Authentications
{
    public class UserContract
    {
        public string UserName { get; set; }
        public string LoginCode { get; set; }
        public List<PermissionType> Permissions { get; set; }

        [HttpKey(Perfix = "; path=/ ;SameSite=None;Secure;HttpOnly")]
        public string Key { get; set; }
    }
}