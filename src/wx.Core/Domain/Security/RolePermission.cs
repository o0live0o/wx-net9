namespace wx.Core.Domain.Security;

public class RolePermission : Entity
{
    public int RoleId { get; private set; }

    public int PermissionId { get; private set; }

    private RolePermission() { }

    public static RolePermission Create(int roleId, int permissionId)
    {
        return new RolePermission
        {
            RoleId = roleId,
            PermissionId = permissionId
        };
    }
}