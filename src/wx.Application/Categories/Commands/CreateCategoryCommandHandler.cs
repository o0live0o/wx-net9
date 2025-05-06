using wx.Core.SeedWork;

namespace wx.Application.Categories;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    private IRepository<Category> _repository;

    public CreateCategoryCommandHandler(IRepository<Category> repository)
    {
        _repository = repository;
    }

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
        _repository.Add(category);
        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return CategoryDto.FromEntity(category);
    }
}