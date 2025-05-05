namespace wx.Application.Categories;

public record DeleteCategoryCommand(int Id) : IRequest<bool>;