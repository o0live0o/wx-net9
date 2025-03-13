namespace wx.Application.Features.Categories;
public record CreateCategoryCommand(string Name, int? ParentId) : IRequest<Unit>;