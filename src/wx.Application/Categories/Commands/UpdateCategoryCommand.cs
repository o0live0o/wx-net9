namespace wx.Application.Categories;

public record UpdateCategoryCommand(int CategoryId, string CategoryName) : IRequest<bool>;
//public record UpdateCategoryCommand(CategoryDto Dto) : IRequest<bool>;