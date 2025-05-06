using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wx.Application.Categories;

public class UpdateCategoryCommandHandler(WxContext context) : IRequestHandler<UpdateCategoryCommand, bool>
{
    public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await context.Categories.Include(p => p.Attributes).SingleOrDefaultAsync(p => p.Id == request.CategoryId)
            ?? throw new KeyNotFoundException();
        category.Update(request.CategoryName);
        await context.SaveEntitiesAsync();
        return true;
    }
}
