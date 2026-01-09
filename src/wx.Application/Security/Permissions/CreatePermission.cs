using Microsoft.Extensions.Logging;
using wx.Core.Domain.Security;
using wx.Core.Enums;
using wx.Core.IRepositories;

namespace wx.Application.Security.Permissions;

public record CreatePermissionCommand(string Code, string Name, PermissionType Type) : IRequest<Unit>;

public class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommand>
{
    public CreatePermissionCommandValidator(ILogger<CreatePermissionCommandValidator> logger)
    {
        RuleFor(command => command.Code).NotEmpty();
        RuleFor(command => command.Name).NotEmpty();
        RuleFor(command => command.Type).IsInEnum();
    }
}

public class CreatePermissionCommandHandler(IPermissionRepository repository) : IRequestHandler<CreatePermissionCommand, Unit>
{
    public async Task<Unit> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
    {
        var exists = await repository.GetAsync(p => p.Code == request.Code);
        if (exists?.Any() == true) throw new ConflictException("Role already exists");

        var permission = Permission.Create(request.Code, request.Name, request.Type);
        await repository.AddAsync(permission);
        return Unit.Value;
    }
}