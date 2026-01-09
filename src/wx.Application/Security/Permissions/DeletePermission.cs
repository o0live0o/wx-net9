using wx.Application.Common.Exceptions;
using wx.Core.IRepositories;

namespace wx.Application.Security.Permissions;

public record DeletePermissionCommand(int Id) : IRequest<Unit>;


public class DeletePermissionCommandHandler(IPermissionRepository repository) : IRequestHandler<DeletePermissionCommand, Unit>
{
    public async Task<Unit> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
    {
        var permission = await repository.GetByIdAsync(request.Id);

        if (permission == null) throw new BadRequestException($"Permission: {request.Id} not found.");
        
        permission.Deactivate();
        await repository.UpdateAsync(permission);

        return Unit.Value;
    }
}