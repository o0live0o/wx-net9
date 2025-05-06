namespace wx.Application.Categories;

public record DeleteCategoryAttrCommandHandler(WxContext context) : IRequestHandler<DeleteCategoryAttrCommand, bool>
{
    public async Task<bool> Handle(DeleteCategoryAttrCommand request, CancellationToken cancellationToken)
    {
        var category = await context.Categories.Include(p => p.Attributes).SingleOrDefaultAsync(p => p.Id == request.CatrgoryId) ?? throw new KeyNotFoundException();
        category.DeleteAttribute(request.AttrId);
        await context.SaveEntitiesAsync();
        return true;
    }
}
