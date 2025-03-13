using wx.Core.SeedWork;

namespace wx.Application.Features.Categories;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Unit>
{
    private IRepository<Category> _repository;

    public CreateCategoryCommandHandler(IRepository<Category> repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        Category order = new Category(request.Name,request.ParentId);
        _repository.Add(order);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}