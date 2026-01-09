namespace wx.Core.Domain.Security;

public class Role : Entity, IAggregateRoot
{
    public string Code { get; private set; }

    public string Name { get; private set; }

    public bool IsActive { get; private set; } = true;

    private List<Menu> _menus = new List<Menu>();

    public IReadOnlyCollection<Menu> Menus => _menus;

    private List<Permission> _permissions = new List<Permission>();

    public IReadOnlyCollection<Permission> Permissions => _permissions;

    private Role()
    {
    }

    public void AddMenu(Menu menu)
    {
        if (_menus.Contains(menu)) return;
        _menus.Add(menu);
    }

    public void AddPermission(Permission permission)
    {
        if (_permissions.Contains(permission)) return;
        _permissions.Add(permission);
    }

    public void Deactive()
    {
        IsActive = false;
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