namespace wx.Application.Categories;
public record CreateCategoryCommand(int? ParentId, string Name, List<CategoryAttrDto>? CategoryAttrs) : IRequest<CategoryDto>;