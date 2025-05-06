using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using wx.Core.Domain.Events;
using wx.Core.Enums;

namespace wx.Core.Entities;

public class Product : Entity, IAggregateRoot
{
    private static readonly Dictionary<string, Action<Product, object>> PropertySetters = new();
    public string ProductNo { get; private set; }
    public string Name { get; private set; }
    public ProductBrandVO BrandModel { get; private set; }
    public string Description { get; private set; }
    public ProductStatus Status { get; private set; } = ProductStatus.None;
    private readonly List<ProductImage> _images = new();
    public IReadOnlyCollection<ProductImage> Images => _images.AsReadOnly();
    public DateTime CreateTime { get; private set; } = DateTime.Now;
    public int CategoryId { get; private set; }
    public Category Category { get; private set; }
    private readonly List<ProductAttribute> _attributes = new();
    public IReadOnlyCollection<ProductAttribute> Attributes => _attributes.AsReadOnly();

    [ConcurrencyCheck]
    public int Version { get; private set; }

    protected Product()
    {
        Version = 1;
    }

    public Product(string productNo, string name, int categoryId, ProductBrandVO brand, string description)
    {
        ProductNo = productNo;
        Name = name;
        CategoryId = categoryId;
        Description = description;
        BrandModel = brand;
        ValidateProductFields();
        Version = 1;
        AddProductCreatedDomainEvent();
    }

    public ProductAttribute SetAttribute(string key, string value)
    {
        var attribute = _attributes.FirstOrDefault(p => p.Key == key);
        if (attribute == null)
        {
            attribute = new ProductAttribute(key, value);
            _attributes.Add(attribute);
        }
        else
        {
            attribute.Update(value);
        }
        Version++;
        return attribute;
    }

    public void UpdateAttribute(int attrId, string value)
    {
        var attribute = _attributes.FirstOrDefault(p => p.Id == attrId);
        if (attribute == null)
            return;
        attribute.Update(value);
    }

    public void Update(string name = null, string description = null, ProductBrandVO brand = null)
    {
        if (name != null)
        {
            Name = name;
        }
        if (description != null)
        {
            Description = description;
        }
        if (brand != null)
        {
            BrandModel = brand;
        }
        Version++;
    }

    public void RemoveAttr(int attrId)
    {
        var attribute = _attributes.FirstOrDefault(a => a.Id == attrId);
        if (attribute != null)
        {
            _attributes.Remove(attribute);
        }
        Version++;
    }

    private void ValidateProductFields()
    {
        if (string.IsNullOrWhiteSpace(ProductNo))
            throw new WxException("Product No. cannot be empty");
        if (string.IsNullOrWhiteSpace(Name))
            throw new WxException("Name cannot be empty");
    }

    public void SetStatus(ProductStatus newStatus)
    {
        Status &= newStatus;
        Version++;
    }

    public void AddImage(string url)
    {
        _images.Add(new ProductImage(url));
        Version++;
    }

    private void AddProductCreatedDomainEvent()
    {
        var @event = new ProductCreatedEvent(Guid.NewGuid(), this);
        this.AddDomainEvent(@event);
    }
}