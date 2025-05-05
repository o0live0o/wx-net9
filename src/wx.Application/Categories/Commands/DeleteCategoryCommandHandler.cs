
using wx.Infrastructure;

namespace wx.Application.Categories;

public class DeleteCategoryCommandHandler(WxContext context) : IRequestHandler<DeleteCategoryCommand, bool>
{
    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await context.Categories.SingleOrDefaultAsync(p => p.Id == request.Id)
            ?? throw new KeyNotFoundException();

        context.Categories.Remove(category);
        await context.SaveChangesAsync();

        return true;
    }
}
