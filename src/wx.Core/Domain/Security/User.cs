using System.Collections.ObjectModel;

namespace wx.Core.Domain.Security;

public class User : Entity, IAggregateRoot
{
    public string Account { get; private set; }

    public string Password { get; private set; }

    public bool IsActive { get; private set; } = true;

    private List<Role> _roles = new List<Role>();

    public ReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

    private User() { }

    public void Activate()
    {
        IsActive = true;
    }

    public void AssignRole(Role role)
    {
        if (_roles.Contains(role)) return;
        _roles.Add(role);
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public static User Create(string account, string password)
    {
        var user = new User();
        user.Account = account;
        user.Password = password;
        return user;
    }
}