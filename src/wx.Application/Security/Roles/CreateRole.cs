using Microsoft.Extensions.Logging;
using wx.Core.Domain.Security;
using wx.Core.IRepositories;

namespace wx.Application.Security.Roles;

public record CreateRoleCommand(string Name, string Code) : IRequest<Unit>;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator(ILogger<CreateRoleCommandValidator> logger)
    {
        RuleFor(command => command.Name).NotEmpty();
        RuleFor(command => command.Code).NotEmpty();
    }
}

public class CreateRoleCommandHandler(IRoleRepository repository) : IRequestHandler<CreateRoleCommand, Unit>
{
    public async Task<Unit> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var exists = await repository.GetAsync(p => p.Code == request.Code);

        if (exists?.Any() == true) throw new ConflictException("Role already exists");

        var role = Role.Create(request.Code, request.Name);
        await repository.AddAsync(role);
        return Unit.Value;
    }
}