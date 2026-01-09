namespace wx.Application.Categories;

public record DeleteCategoryAttrCommandHandler(IRepository<Category> repository) : IRequestHandler<DeleteCategoryAttrCommand, bool>
{
    public async Task<bool> Handle(DeleteCategoryAttrCommand request, CancellationToken cancellationToken)
    {
        var category = await repository.GetByIdAsync(request.CatrgoryId) ?? throw new KeyNotFoundException();
        category.DeleteAttribute(request.AttrId);
        await repository.UpdateAsync(category);
        return true;
    }
}
