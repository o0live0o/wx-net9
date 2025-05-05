using System.ComponentModel.DataAnnotations;

namespace wx.Core.Entities;

public class Category : Entity, IAggregateRoot
{
    public string Name { get; private set; }

    public int? ParentId { get; private set; }

    public int Order { get; private set; }

    [ConcurrencyCheck]
    public int Version { get; private set; }

    public Category Parent { get; private set; }

    private List<Category> _childrens;

    private List<CategoryAttribute> _attributes;

    public IReadOnlyCollection<Category> Childrens => _childrens.AsReadOnly();

    public IReadOnlyCollection<CategoryAttribute> Attributes => _attributes.AsReadOnly();

    protected Category()
    {
        _childrens = new List<Category>();
        _attributes = new List<CategoryAttribute>();
        Version = 1;
    }

    public Category(string name, int? parentId = null)
    {
        Name = name;
        ParentId = parentId;
        _attributes = new List<CategoryAttribute>();
        _childrens = new List<Category>();
        Order = 0;
        Version = 1;
    }

    public void Update(string name)
    {
        Name = name;
        Version++;
    }

    public CategoryAttribute AddAttribute(string attrName)
    {
        if (_attributes.FirstOrDefault(p => p.Name.Equals(attrName)) != null)
            throw new WxException($"Category Attribute: {attrName} already exists.");

        var attribute = new CategoryAttribute(attrName, Id);
        _attributes.Add(attribute);
        Version++;
        return attribute;
    }

    public void DeleteAttribute(int attrId)
    {
        var attr = _attributes.SingleOrDefault(p => p.Id == attrId);
        if (attr != null)
        {
            _attributes.Remove(attr);
            Version++;
        }
    }

    public void UpdateAttribute(int attrId, string attrName)
    {
        var attr = _attributes.SingleOrDefault(p => p.Id == attrId);

        if (attr == null)
            throw new KeyNotFoundException($"Category ID {attrId} not found.");

        attr.UpdateName(attrName);
        Version++;
    }
}
