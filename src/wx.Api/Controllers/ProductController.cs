using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using wx.Application.Products;
using wx.Application.Products.Commands;
using wx.Core;
using wx.Core.Entities;

namespace wx.Api.Controllers;

public class ProductController(IMediator mediator, IProductQuery productQuery) : WxController
{
    private readonly string[] allowedTypes = new[] { ".jpg", ".jpeg", ".png" };

    #region Product
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<ProductDto>), StatusCodes.Status200OK)]
    public async Task<PaginatedList<ProductDto>> GetProducts([FromQuery] PaginationRequest request)
    {
        var products = await productQuery.GetProducts(request);
        return products;
    }

    [HttpGet("{productId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct([FromRoute] int productId)
    {
        var product = await productQuery.GetProduct(productId);
        if (product == null)
            return NotFound($"Product with id {productId} not found"); ;
        return Ok(product);
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct([FromHeader(Name = "x-requestid")] Guid requestId, [FromBody] CreateProductRequest request)
    {
        CreateProductCommand command = new CreateProductCommand(request.Brand,
                                                                request.Name,
                                                                request.CategoryId,
                                                                request.Model,
                                                                request.Description,
                                                                request.ProductAttrs);
        var product = await mediator.Send(command);

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }


    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct([FromHeader(Name = "x-requestid")] Guid requestId, [FromRoute] int id, [FromBody] UpdateProductRequest request)
    {
        UpdateProductCommand command = new UpdateProductCommand(id, request.Name, request.Brand, request.Model, request.Description);
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct([FromRoute] int id)
    {
        await mediator.Send(new DeleteProductCommand(id));
        return NoContent();
    }
    #endregion

    #region Product Attribute
    [HttpGet("{productId:int}/attr")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ProductAttributeDto>>> GetProductAttributes([FromRoute] int productId)
    {
        var attributes = await productQuery.GetProductAttributes(productId);
        if (attributes == null)
            return NotFound($"Product with id {productId} not found");
        return Ok(attributes);
    }

    [HttpPost("{productId:int}/attr")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddProductAttribute(
        [FromRoute] int productId,
        [FromHeader(Name = "x-requestid")] Guid requestId,
        [FromBody] AddProductAttrRequest request)
    {
        var command = new AddProductAttrCommand(productId, request.AttrId, request.Value);
        var attribute = await mediator.Send(command);
        return CreatedAtAction(nameof(GetProductAttributes), new { productId }, attribute);
    }

    [HttpPut("{productId:int}/attr/{attrId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProductAttribute(
        [FromRoute] int productId,
        [FromRoute] int attrId,
        [FromBody] UpdateProductAttrRequest request)
    {
        var command = new UpdateProductAttrCommand(productId, attrId, request.Value);
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{productId:int}/attr/{attributeId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProductAttribute(
        [FromRoute] int productId,
        [FromRoute] int attributeId)
    {
        await mediator.Send(new DeleteProductAttrCommand(productId, attributeId));
        return NoContent();
    }
    #endregion

    #region Image
    [HttpPost("{id:int}/image")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadProductImage([FromRoute] int id,
                                             [FromHeader(Name = "x-requestid")] Guid requestId,
                                             IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedTypes.Contains(extension))
            return BadRequest($"Invalid file type. Allowed types are: {string.Join(", ", allowedTypes)}");

        if (file.Length > 5 * 1024 * 1024)
            return BadRequest("File size should not exceed 5MB");

        var command = new UploadProductImageCommand(id, file.OpenReadStream(), file.FileName);
        await mediator.Send(command);

        return Ok(new { imagePath = $"/images/Img{id}" });
    }

    #endregion
}
