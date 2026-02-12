namespace wx.Core.Domain.Security;

public class Menu : Entity, IAggregateRoot
{
    public string Name { get; set; }

    public string Code { get; set; }

    public int ParentId { get; set; }

    public string? Description { get; set; }

    public static Menu Create(string name, string? description = null, int parentId = 0)
    {
        return new Menu
        {
            Name = name,
            Description = description,
            ParentId = parentId,
        };
    }
}