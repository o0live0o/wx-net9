namespace wx.Application.Products;

public record UpdateProductRequest(string? Name, string? Brand, string? Model, string? Description);

public record UpdateProductCommand(int Id, string? Name, string? Brand, string? Model, string? Description) : IRequest<bool>;

public record UpdateProductAttributeDto(int? Id,  int AttributeId, string Value);
