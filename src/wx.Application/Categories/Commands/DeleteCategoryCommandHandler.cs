namespace wx.Application.Categories;

public class DeleteCategoryCommandHandler(IRepository<Category> repository) : IRequestHandler<DeleteCategoryCommand, bool>
{
    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await repository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException();

        await repository.DeleteAsync(category);
        return true;
    }
}
