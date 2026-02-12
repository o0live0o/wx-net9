using wx.Core.Domain.Security;
using wx.Core.IRepositories;

namespace  wx.Application.Security.Menus;

public class CreateMenuCommandHandler(IMenuRepository repository) : IRequestHandler<CreateMenuCommand, Unit>
{
    public async Task<Unit> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
    {
        var menu = Menu.Create(request.Name, request.Description, request.ParentId);
        await repository.AddAsync(menu);
        return Unit.Value;

    }
}