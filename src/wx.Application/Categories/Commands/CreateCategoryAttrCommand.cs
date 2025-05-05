namespace wx.Application.Categories;

public class CreateCategoryAttrCommand : IRequest<CategoryAttrDto>
{
    public int CategoryId { get; set; }
    public CategoryAttrDto Attr { get; set; }
}