namespace wx.Application.Categories;

public record DeleteCategoryAttrCommand(int CatrgoryId, int AttrId) : IRequest<bool>;