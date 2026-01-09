using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wx.Application.Categories;

public class UpdateCategoryCommandHandler(IRepository<Category> repository) : IRequestHandler<UpdateCategoryCommand, bool>
{
    public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await repository.GetByIdAsync(request.CategoryId)
            ?? throw new KeyNotFoundException();
        category.Update(request.CategoryName);
        await repository.UpdateAsync(category);
        return true;
    }
}
