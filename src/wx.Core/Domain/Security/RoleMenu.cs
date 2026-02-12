namespace wx.Core.Domain.Security;

public class RoleMenu : Entity
{
    public int RoleId { get; private set; }

    public int MenuId { get; private set; }

    private RoleMenu() { }

    public static RoleMenu Create(int roleId, int menuId)
    {
        return new RoleMenu
        {
            RoleId = roleId,
            MenuId = menuId,
        };
    }
}