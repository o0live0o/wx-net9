using wx.Application.Categories;
using wx.Application.Common.Models;

namespace wx.Application.Categories;

public interface ICategoryQuery
{
    Task<PaginatedList<CategoryDto>> GetCategoriesAsync(PaginationRequest request);
    Task<CategoryDto> GetCategoryByIdAsync(int id);
    Task<IEnumerable<Category>> GetCategoryWithChildByIdAsync(int id);
}
