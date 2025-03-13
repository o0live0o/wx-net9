namespace wx.Core.Entities;

public class Category : Entity, IAggregateRoot
{
    public string Name { get; private set; }

    public int? ParentId { get; private set; }

    public int Order { get; private set; }

    public Category Parent { get; private set; }

    private List<Category> _childrens;

    public IReadOnlyCollection<Category> Childrens => _childrens.AsReadOnly();

    protected Category()
    {
        _childrens = new List<Category>();
    }

    public Category(string name, int? parentId = null)
    {
        Name = name;
        ParentId = parentId;
    }
}
