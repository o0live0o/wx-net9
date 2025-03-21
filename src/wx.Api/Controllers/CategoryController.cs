﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wx.Application.Fearures.Categories;
using wx.Application.Features.Categories;

namespace wx.Api.Controllers;

[Authorize]
public class CategoryController(IMediator mediator, ICategoryQuery categoryQuery, ILogger<CategoryController> logger, IHttpContextAccessor contextAccessor) : WxController
{

    [HttpGet("{id:int}")]
    public async Task<IActionResult> QueryByIdAsync([FromRoute] int id)
    {
        var category = await categoryQuery.GetCategoryByIdAsync(id);
        return Ok(category);
    }

    [HttpGet]
    [Authorize(Roles = "Super-Admin,Admin1,Normal")]

    public async Task<IActionResult> QueryAsync()
    {
        var user = contextAccessor.HttpContext.User;
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
