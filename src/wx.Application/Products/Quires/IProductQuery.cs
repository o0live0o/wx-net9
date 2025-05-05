namespace wx.Application.Products;

public interface IProductQuery
{
    Task<ProductDto> GetProduct(int productId);

    Task<PaginatedList<ProductDto>> GetProducts(PaginationRequest request);

    Task<IEnumerable<ProductAttributeDto>> GetProductAttributes(int productId);
}
