namespace wx.Application.Categories;

public class CreateCategoryAttrCommandHandler(IRepository<Category> repository) : IRequestHandler<CreateCategoryAttrCommand, CategoryAttrDto>
{
    public async Task<CategoryAttrDto> Handle(CreateCategoryAttrCommand request, CancellationToken cancellationToken)
    {
        var category = await repository.GetByIdAsync(request.CategoryId);

        if (category == null)
            throw new KeyNotFoundException();

        var attribute = category.AddAttribute(request.Attr.Name);

        await repository.UpdateAsync(category,cancellationToken);

        return CategoryAttrDto.FromEntity(attribute); ;
    }
}
