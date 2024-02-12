﻿using System.Collections.Generic;
using Translators.Contracts.Common.DataTypes;

namespace Translators.Contracts.Common.Authentications
{
    public class UserContract
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string LoginCode { get; set; }
        public List<PermissionType> Permissions { get; set; }

        public string Key { get; set; }
    }
}