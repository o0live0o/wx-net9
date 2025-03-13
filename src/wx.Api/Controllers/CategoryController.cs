using MediatR;
using Microsoft.AspNetCore.Mvc;
using wx.Application.Fearures.Categories;
using wx.Application.Features.Categories;

namespace wx.Api.Controllers;

public class CategoryController(IMediator mediator, ICategoryQuery categoryQuery, ILogger<CategoryController> logger) : WxController
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> QueryByIdAsync([FromRoute] int id)
    {
        var category = await categoryQuery.GetCategoryByIdAsync(id);
        return Ok(category);
    }

    [HttpGet]
    public async Task<IActionResult> QueryAsync()
    {
        var categories = await categoryQuery.GetCategoriesAsync();
        return Ok(categories);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateCategoryCommand command)
    {
        logger.LogInformation(
            "Sending command: {CommandName} - ({@Command})",
            command.GetType().Name,
            command);
        await mediator.Send(command);
        return Ok();
    }
}
