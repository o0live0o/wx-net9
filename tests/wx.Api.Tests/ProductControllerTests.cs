using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wx.Api.Controllers;
using wx.Application.Products;
using wx.Core;

namespace wx.Api.Tests;

public class ProductControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IProductQuery> _productQueryMock;
    private readonly ProductController _controller;
    public ProductControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _productQueryMock = new Mock<IProductQuery>();
        _controller = new ProductController(_mediatorMock.Object, _productQueryMock.Object);
    }

    [Fact]
    public async Task GetProducts_ReturnsPaginatedList()
    {
        var request = new PaginationRequest();
        var expected = new PaginatedList<ProductDto>(new List<ProductDto>(), 0, 1, 10);
        _productQueryMock.Setup(x => x.GetProducts(request)).ReturnsAsync(expected);

        var result = await _controller.GetProducts(request);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetProducts_WithNullRequest_ReturnsDefaultPaginatedList()
    {
        PaginationRequest request = null;
        var expected = new PaginatedList<ProductDto>(new List<ProductDto>(), 0, 1, 10);
        _productQueryMock.Setup(x => x.GetProducts(It.IsAny<PaginationRequest>())).ReturnsAsync(expected);

        var result = await _controller.GetProducts(request);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetProduct_ExistingProductId_ReturnsProduct()
    {
        var productId = 1;
        var expectedProduct = new ProductDto { Id = productId, Name = "Product" };
        _productQueryMock.Setup(x => x.GetProduct(productId)).ReturnsAsync(expectedProduct);

        var result = await _controller.GetProduct(productId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualProduct = Assert.IsType<ProductDto>(okResult.Value);
        Assert.Equal(expectedProduct.Id, actualProduct.Id);
        Assert.Equal(expectedProduct.Name, actualProduct.Name);
    }
}
