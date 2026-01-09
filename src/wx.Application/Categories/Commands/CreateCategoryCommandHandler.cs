using wx.Core.SeedWork;

namespace wx.Application.Categories;

public class CreateCategoryCommandHandler(IRepository<Category> repository) : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        Category category = new Category(request.Name, request.ParentId);

        if (request.CategoryAttrs?.Count > 0)
        {
            foreach (var attr in request.CategoryAttrs)
            {
                category.AddAttribute(attr.Name);
            }
        }
        await repository.AddAsync(category);
        return CategoryDto.FromEntity(category);
    }
}