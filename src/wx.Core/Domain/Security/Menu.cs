namespace wx.Core.Domain.Security;

public class Menu : Entity, IAggregateRoot
{
    public string Name { get; set; }

    public string Code { get; set; }
}