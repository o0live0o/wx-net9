using System.Collections.ObjectModel;
using System.Globalization;

namespace wx.Core.Domain.Security;

public class User : Entity, IAggregateRoot
{
    public string Account { get; init; } = null!;

    public string Password { get; private set; } = null!;

    public bool IsActive { get; private set; } = true;

    private ICollection<UserRole> _userRoles = new Collection<UserRole>();

    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.ToList().AsReadOnly();

    private User() { }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void AssignRole(int roleId)
    {
        if(_userRoles.Any(p => p.RoleId == roleId)) return;
        _userRoles.Add(UserRole.Create(Id, roleId));
    }

    public static User Create(string account, string password)
    {
        var user = new User
        {
            Account = account,
            Password = password,
        };
        return user;
    }
}