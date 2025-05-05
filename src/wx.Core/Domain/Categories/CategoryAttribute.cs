using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wx.Core.Entities;

public class CategoryAttribute : Entity
{
    public string Name { get; private set; }
    public int CategoryId { get; private set; }
    public Category Category { get; private set; }

    public CategoryAttribute(string name, int categoryId)
    {
        Name = name;
        CategoryId = categoryId;
    }

    public void UpdateName(string name)
    {
        Name = name;
    }
}
