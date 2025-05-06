using Azure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using wx.Core;
using wx.Core.SeedWork;
using wx.Infrastructure;

namespace wx.Application.Categories;

public class CategoryQuery(WxContext context) : ICategoryQuery
{
    public async Task<PaginatedList<CategoryDto>> GetCategoriesAsync(PaginationRequest request)
    {
        var query = context.Categories.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(x => x.Name.Contains(request.SearchTerm));
        }

        query = ApplyOrder(query, request);

        var totalCount = await query.CountAsync();

        var items = await query
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(c => new CategoryDto(c.Id, c.Name, c.ParentId, null))
                    .ToListAsync();

        return new PaginatedList<CategoryDto>(items, totalCount, request.PageIndex, request.PageSize);
    }

    public async Task<CategoryDto> GetCategoryByIdAsync(int id)
    {
        var category = await context.Categories.AsNoTracking().Include(p => p.Attributes).FirstOrDefaultAsync(p => p.Id == id);

        if (category is null)
            throw new KeyNotFoundException();

        CategoryAttrDto[] categoryAttrs = category.Attributes?.Select(p => new CategoryAttrDto(p.Id, p.Name))?.ToArray();
        return new CategoryDto(category.Id, category.Name, category.ParentId, categoryAttrs);
    }

    public async Task<IEnumerable<Category>> GetCategoryWithChildByIdAsync(int id)
    {
        var query = @"
            WITH CategoryTree AS (
                SELECT * FROM Categories WHERE Id = {0}
                UNION ALL
                SELECT c.* FROM Categories c
                INNER JOIN CategoryTree ct ON c.ParentId = ct.Id
            )
            SELECT * FROM CategoryTree"
       ;
        var categories = await context.Categories
                    .FromSqlRaw(query, id)
                    .ToListAsync();
        return categories;
    }

    private static IQueryable<Category> ApplyOrder(IQueryable<Category> query, PaginationRequest request)
    {
        return request.OrderBy?.ToLower() switch
        {
            "name" => request.Descending
                ? query.OrderByDescending(x => x.Name)
                : query.OrderBy(x => x.Name),
            _ => query.OrderBy(x => x.Id)
        };
    }
}
