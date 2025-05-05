using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wx.Application.Categories;

public class UpdateCategoryAttrCommand : IRequest<bool>
{
    public int CategoryId { get; set; }
    public int AttrId { get; set; }
    public string AttrName { get; set; }
}

public record UpdateCategoryAttrRequest(string AttrName);