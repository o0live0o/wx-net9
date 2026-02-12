namespace wx.Core.Domain.Security;

public class UserRole : Entity
{
    public int UserId { get; private set; }
    public int RoleId { get; private set; }

    private UserRole() { }

    public static UserRole Create(int userId, int roleId)
    {
        var userRole = new UserRole();
        userRole.UserId = userId;
        userRole.RoleId = roleId;
        return userRole;
    }
}