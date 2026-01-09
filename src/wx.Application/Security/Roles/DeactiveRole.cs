using wx.Core.IRepositories;

namespace wx.Application.Security.Roles;

public record DeactiveRoleCommand(int Id) : IRequest<Unit>;

public class DeactiveRoleCommandHandler(IRoleRepository repository) : IRequestHandler<DeactiveRoleCommand, Unit>
{
    public async Task<Unit> Handle(DeactiveRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await repository.GetByIdAsync(request.Id);
        if (role == null)
            throw new BadRequestException($"Role  with id {request.Id} not found");
        
        role.Deactive();
        await repository.UpdateAsync(role);
        return Unit.Value;
    }
}