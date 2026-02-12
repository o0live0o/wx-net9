using wx.Core.IRepositories;

namespace wx.Application.Security.Roles;

public record AssignPermissionCommand(int RoleId, int[] PermissionIds) : IRequest<Unit>;

public class AssignPermissionCommandValidator : AbstractValidator<AssignPermissionCommand>
{
    public AssignPermissionCommandValidator()
    {
        RuleFor(x => x.RoleId).NotNull().Must(a => a > 0);

        RuleFor(x => x.PermissionIds)
            .NotNull().WithMessage("PermissionIds不能为空")
            .Must(a => a.Length > 0).WithMessage("至少提供一个 PermissionId")
            .Must(a => a.All(id => id > 0)).WithMessage("PermissionIds 必须为正整数")
            .Must(a => a.Distinct().Count() == a.Length).WithMessage("PermissionIds 不能重复");
    }
}

public class AssignPermissionCommandHandler(IRoleRepository repository, IPermissionRepository permissionRepository)
    : IRequestHandler<AssignPermissionCommand, Unit>
{
    public async Task<Unit> Handle(AssignPermissionCommand request, CancellationToken cancellationToken)
    {
        var role = await repository.GetByIdAsync(request.RoleId);
        if (role == null)
            throw new BadRequestException($"Role with id {request.RoleId} not found");

        var permissions = await permissionRepository.GetAsync(p => request.PermissionIds.Contains(p.Id));

        var foundIds = permissions.Select(p => p.Id).ToHashSet();

        var missing = request.PermissionIds.Select(p => !foundIds.Contains(p)).ToArray();
        if (missing.Length > 0)
            throw new BadRequestException($"Permissions not found: {string.Join(',', missing)}");

        foreach (var permissionId in request.PermissionIds)
        {
            role.AssignPermission(permissionId);
        }

        await repository.UpdateAsync(role, cancellationToken);
        return Unit.Value;
    }
}