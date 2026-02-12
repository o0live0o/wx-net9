namespace wx.Core.Domain.Security;

public class Role : Entity, IAggregateRoot
{
    public string Code { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public bool IsActive { get; private set; } = true;

    private ICollection<RolePermission> _rolePermissions = new List<RolePermission>();

    public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.ToList().AsReadOnly();

    private ICollection<RoleMenu> _roleMenus = new List<RoleMenu>();

    public IReadOnlyCollection<RoleMenu> RoleMenus => _roleMenus.ToList().AsReadOnly();

    private Role()
    {
    }

    public void Deactive()
    {
        IsActive = false;
    }

    public void AssignPermission(int permissionId)
    {
        if (_rolePermissions.Any(p => p.PermissionId == permissionId)) return;
        _rolePermissions.Add(RolePermission.Create(Id, permissionId));
    }

    public void AssignMenu(int menuId)
    {
        if (_roleMenus.Any(p => p.MenuId == menuId)) return;
        _roleMenus.Add(RoleMenu.Create(Id, menuId));
    }

    public static Role Create(string code, string name)
    {
        return new Role
        {
            Code = code,
            Name = name
        };
    }
}