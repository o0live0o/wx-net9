using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using wx.Application.Categories;
using wx.Core;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace wx.Api.Controllers;

//[Authorize]
public class CategoryController(IMediator mediator, ICategoryQuery categoryQuery, ILogger<CategoryController> logger, IHttpContextAccessor contextAccessor) : WxController
{
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories([FromQuery] PaginationRequest request)
    {
        var categories = await categoryQuery.GetCategoriesAsync(request);
        return Ok(categories);
    }

    [HttpGet("{categoryId:int}")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryDto>> GetCategory([FromRoute] int categoryId)
    {
        var category = await categoryQuery.GetCategoryByIdAsync(categoryId);
        if (category == null)
            return NotFound();
        return Ok(category);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCategoryAsync(CreateCategoryCommand command)
    {
        logger.LogInformation(
            "Sending command: {CommandName} - ({@Command})",
            command.GetType().Name,
            command);
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(GetCategory), new { id = result.Id }, result);
    }

    [HttpDelete("{categoryId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategoryAsync([FromRoute] int categoryId)
    {
        DeleteCategoryCommand command = new DeleteCategoryCommand(categoryId);
        await mediator.Send(command);
        return NoContent();
    }

    //[HttpPatch("{categoryId:int}")]
    //public async Task<IActionResult> PatchUpdateAsync([FromRoute] int categoryId, [FromBody] JsonPatchDocument<CategoryDto> patchDoc)
    //{
    //    var category = await categoryQuery.GetCategoryByIdAsync(categoryId);
    //    if (category == null)
    //        return NotFound();
    //    patchDoc.ApplyTo(category);
    //    var command = new UpdateCategoryCommand(category);
    //    await mediator.Send(command);
    //    return NoContent();
    //}

    [HttpPut("{categoryId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCategoryAsync([FromRoute] int categoryId, [FromBody] UpdateCategoryCommand request)
    {
        var command = new UpdateCategoryCommand(categoryId, request.CategoryName);
        await mediator.Send(request);
        return NoContent();
    }

    [HttpGet("{categoryId:int}/attr")]
    [ProducesResponseType(typeof(IEnumerable<CategoryAttrDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<CategoryAttrDto>>> GetCategoryAttrs(int categoryId)
    {
        var category = await categoryQuery.GetCategoryByIdAsync(categoryId);
        if (category == null)
            return NotFound();

        return Ok(category.CategoryAttrs);
    }

    [HttpGet("{categoryId:int}/attr/{attributeId:int}")]
    [ProducesResponseType(typeof(CategoryAttrDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryAttrDto>> GetCategoryAttr(int categoryId, int attributeId)
    {
        var category = await categoryQuery.GetCategoryByIdAsync(categoryId);
        if (category == null)
            return NotFound();
        var attribute = category.CategoryAttrs.SingleOrDefault(p => p.AttrId == attributeId);
        if (attribute == null)
            return NotFound();

        return Ok(attribute);
    }

    [HttpPost("{categoryId:int}/attr")]
    [ProducesResponseType(typeof(CategoryAttrDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryAttrDto>> CreateCategoryAttribute(
    [FromRoute] int categoryId,
    [FromBody][Required] CreateCategoryAttrCommand command)
    {
        command.CategoryId = categoryId;
        var result = await mediator.Send(command);

        return CreatedAtAction(
            nameof(GetCategoryAttr),
            new { categoryId, attributeId = result.AttrId },
            result);
    }

    [HttpPut("{categoryId:int}/attr/{attrId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCategoryAttribute(
       [FromRoute] int categoryId,
       [FromRoute] int attrId,
       [FromBody] UpdateCategoryAttrRequest request)
    {
        var command = new UpdateCategoryAttrCommand
        {
            CategoryId = categoryId,
            AttrId = attrId,
            AttrName = request.AttrName
        };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{categoryId:int}/attr/{attrId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAttributeAsync([FromRoute] int categoryId, [FromRoute] int attrId)
    {

        var command = new DeleteCategoryAttrCommand(
            categoryId,
            attrId
        );

        await mediator.Send(command);
        return NoContent();
    }


}