namespace wx.Core.Entities;

public class Category : Entity
{
    public string Name { get; set; }

    public int? ParentId { get; set; }

    public int Order { get; set; }

    public Category Parent { get; set; }
    public ICollection<Category> Children { get; set; }
}
