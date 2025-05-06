using Microsoft.Extensions.Logging;

using wx.Infrastructure;

namespace wx.Application.Products;

public class ProductQuery(WxContext context, ILogger<ProductQuery> logger) : IProductQuery
{
    public async Task<ProductDto> GetProduct(int productId)
    {
        var product = await context.Products.AsNoTracking().Include(p => p.Category).Include(p => p.Attributes).SingleOrDefaultAsync(p => p.Id == productId);

        if (product == null)
            throw new KeyNotFoundException();

        var attributes = product?.Attributes?.Select(a => new ProductAttributeQueryDto
        {
            Id = a.Id,
            Key = a.Key,
            Value = a.Value
        }).ToList();

        var productDto = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Brand = product.BrandModel.Brand,
            Model = product.BrandModel.Model,
            Description = product.Description,
            ProductAttrs = attributes,
            CategoryId = product.CategoryId,
            CategoryName = product.Category.Name,
        };
        return productDto;
    }

    public async Task<PaginatedList<ProductDto>> GetProducts(PaginationRequest request)
    {
        var query = context.Products.AsNoTracking().Include(p => p.Category).AsQueryable();
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(x => x.Description.Contains(request.SearchTerm));
        }

        query = ApplyOrder(query, request);
        var totalCount = await query.CountAsync();


        var items = await  query.Skip((request.PageIndex - 1) * request.PageSize)
                                .Take(request.PageSize)
                                .Select(p => new ProductDto() { 
                                    Id = p.Id,
                                    Name = p.Name,
                                    Brand = p.BrandModel.Brand,
                                    Model = p.BrandModel.Model,
                                    CategoryId = p.CategoryId,
                                    CategoryName = p.Category.Name,
                                    Description = p.Description
                                })
                                .ToListAsync();
        return new PaginatedList<ProductDto>(items, totalCount, request.PageIndex, request.PageSize); ;
    }

    public async Task<IEnumerable<ProductAttributeDto>> GetProductAttributes(int productId)
    {
        var product = await context.Products.AsNoTracking().Include(p => p.Category).Include(p => p.Attributes).SingleOrDefaultAsync(p => p.Id == productId);

        if (product == null)
            throw new KeyNotFoundException();

        return product.Attributes?.Select(p => ProductAttributeDto.FromEntity(p));
    }

    private static IQueryable<Product> ApplyOrder(IQueryable<Product> query, PaginationRequest request)
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
