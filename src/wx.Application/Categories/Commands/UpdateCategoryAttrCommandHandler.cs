namespace wx.Application.Categories;

public class UpdateCategoryAttrCommandHandler(IRepository<Category> repository) : IRequestHandler<UpdateCategoryAttrCommand, bool>
{
    public async Task<bool> Handle(UpdateCategoryAttrCommand request, CancellationToken cancellationToken)
    {
        var category = await repository.GetByIdAsync(request.CategoryId) ??
            throw new KeyNotFoundException();
        
        category.UpdateAttribute(request.AttrId, request.AttrName);
        await repository.UpdateAsync(category);
        return true;
    }
}
