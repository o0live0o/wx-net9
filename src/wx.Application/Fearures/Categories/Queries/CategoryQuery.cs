using Microsoft.EntityFrameworkCore;
using wx.Core.SeedWork;
using wx.Infrastructure;

namespace wx.Application.Fearures.Categories;

public class CategoryQuery(WxContext context) : ICategoryQuery
{
    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync() =>
        await context.Categories.Select(c => new CategoryDto { Id = c.Id, Name = c.Name }).ToListAsync();

    public async Task<CategoryDto> GetCategoryByIdAsync(int id)
    {
        var category = await context.Categories.FirstOrDefaultAsync(p => p.Id == id);

        if (category is null)
            throw new KeyNotFoundException();

        return new CategoryDto { Id = category.Id, Name = category.Name };
    }
}
