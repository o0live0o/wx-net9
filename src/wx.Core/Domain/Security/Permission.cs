using wx.Core.Enums;

namespace wx.Core.Domain.Security;

public class Permission : Entity, IAggregateRoot
{
    public string Name { get; private set; }

    public string Code { get; private set; }

    public PermissionType Type { get; private set; }

    public bool IsActive { get; private set; } = true;

    private Permission()
    {
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public static Permission Create(string code, string name, PermissionType type)
    {
        return new Permission
        {
            Code = code,
            Name = name,
            Type = type
        };
    }
}