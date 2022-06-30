using Translators.Contracts.Common.DataTypes;

namespace Translators.Database.Entities.Authentications
{
    public class UserPermissionEntity
    {
        public long Id { get; set; }
        public PermissionType PermissionType { get; set; }
        public long UserId { get; set; }
        public UserEntity User { get; set; }
    }
}
