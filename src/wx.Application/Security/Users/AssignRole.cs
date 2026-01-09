using wx.Core.IRepositories;

namespace wx.Application.Security.Users;

public record AssignRolesCommand(int UserId, int[] RoleIds) : IRequest<Unit>;

public class AssignRolesCommandValidator : AbstractValidator<AssignRolesCommand>
{
    public AssignRolesCommandValidator()
    {
        RuleFor(x => x.RoleIds)
            .NotNull().WithMessage("Roles不能为空")
            .Must(a => a.Length > 0)
            .Must(a => a.All(id => id > 0))
            .Must(a => a.Distinct().Count() == a.Length);
    }
}

public class AssignRolesCommandHandler(IUserRepository repository, IRoleRepository roleRepository)
    : IRequestHandler<AssignRolesCommand, Unit>
{
    public async Task<Unit> Handle(AssignRolesCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.GetByIdAsync(request.UserId);

        if (user == null)
            throw new BadRequestException($"User: {request.UserId} not found.");
        
        var roleIdSet = request.RoleIds.ToHashSet();
        var roles = await roleRepository.GetAsync(p => roleIdSet.Contains(p.Id));

        var foundIds = roles.Select(p => p.Id).ToHashSet();
        var missingIds = request.RoleIds.Where(p => !foundIds.Contains(p)).ToArray();

        if (missingIds.Length > 0)
            throw new BadRequestException($"Role not found: {string.Join(',', missingIds)}");

        foreach (var role in roles)
        {
            user.AssignRole(role);
        }

        await repository.UpdateAsync(user);
        return Unit.Value;
    }
}