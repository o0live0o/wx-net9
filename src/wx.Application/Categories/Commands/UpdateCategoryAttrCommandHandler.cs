namespace wx.Application.Categories;

public class UpdateCategoryAttrCommandHandler(WxContext context) : IRequestHandler<UpdateCategoryAttrCommand, bool>
{
    public async Task<bool> Handle(UpdateCategoryAttrCommand request, CancellationToken cancellationToken)
    {
        var category = await context.Categories.SingleOrDefaultAsync(p => p.Id == request.CategoryId) ??
            throw new KeyNotFoundException();
        
        category.UpdateAttribute(request.AttrId, request.AttrName);
        await context.SaveChangesAsync();
        return true;
    }
}
