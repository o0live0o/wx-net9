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

    public string Key { get; private set; }

    public string Value { get; set; }

    protected ProductAttribute() { }

    public ProductAttribute(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public void Update(string value)
    {
        Value = value;
    }
}
