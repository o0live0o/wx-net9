
namespace wx.Application.Fearures.Categories;

public interface ICategoryQuery
{
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
    Task<CategoryDto> GetCategoryByIdAsync(int id);
}
