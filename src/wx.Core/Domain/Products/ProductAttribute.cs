using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace wx.Core.Entities;

public class ProductAttribute : Entity
{
    public int ProductId { get; private set; }
    [JsonIgnore]
    public Product Product { get; private set; }
    public int CategoryAttributeId {  get; set; }
    [JsonIgnore]
    public CategoryAttribute CategoryAttribute { get; set; }
    public string Value { get; set; }

    protected ProductAttribute() { }

    public ProductAttribute(int categoryAttributeId, string value)
    {
        CategoryAttributeId = categoryAttributeId;
        Value = value;
    }

    public void Update(string value)
    {
        Value = value;
    }
}
