using wx.Core.IRepositories;

namespace wx.Application.Security.Roles;

public record AssignMenuCommand(int RoleId, int[] MenuIds) : IRequest<Unit>;

public class AssignMenuCommandValidator : AbstractValidator<AssignMenuCommand>
{
    public AssignMenuCommandValidator()
    {
        RuleFor(x => x.RoleId).NotNull().Must(a => a > 0);

        RuleFor(x => x.MenuIds)
            .NotNull().WithMessage("MenuIds不能为空")
            .Must(a => a.Length > 0).WithMessage("至少提供一个 MenuId")
            .Must(a => a.All(id => id > 0)).WithMessage("MenuIds 必须为正整数")
            .Must(a => a.Distinct().Count() == a.Length).WithMessage("MenuIds 不能重复");
    }
}

public class AssignMenuCommandHandler(IRoleRepository repository, IMenuRepository menuRepository)
    : IRequestHandler<AssignMenuCommand, Unit>
{
    public async Task<Unit> Handle(AssignMenuCommand request, CancellationToken cancellationToken)
    {
        var role = await repository.GetByIdAsync(request.RoleId);
        if (role == null)
            throw new BadRequestException($"Role with id {request.RoleId} not found");

        var menus = await menuRepository.GetAsync(p => request.MenuIds.Contains(p.Id));

        var foundIds = menus.Select(p => p.Id).ToHashSet();

        var missing = request.MenuIds.Select(p => !foundIds.Contains(p)).ToArray();
        if (missing.Length > 0)
            throw new BadRequestException($"Permissions not found: {string.Join(',', missing)}");

        foreach (var menuId in request.MenuIds)
        {
            role.AssignPermission(menuId);
        }

        await repository.UpdateAsync(role, cancellationToken);
        return Unit.Value;
    }
}