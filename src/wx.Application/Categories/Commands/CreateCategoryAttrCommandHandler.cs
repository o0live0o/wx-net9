namespace wx.Application.Categories;

public class CreateCategoryAttrCommandHandler(WxContext context) : IRequestHandler<CreateCategoryAttrCommand, CategoryAttrDto>
{
    public async Task<CategoryAttrDto> Handle(CreateCategoryAttrCommand request, CancellationToken cancellationToken)
    {
        var category = await context.Categories.Include(c => c.Attributes).SingleOrDefaultAsync(p => p.Id == request.CategoryId);

        if (category == null)
            throw new KeyNotFoundException();

        var attribute = category.AddAttribute(request.Attr.Name);

        await context.SaveChangesAsync();

        return CategoryAttrDto.FromEntity(attribute); ;
    }
}
