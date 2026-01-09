using Microsoft.Extensions.Logging;
using wx.Application.Common.Interfaces;
using wx.Core.Domain.Security;
using wx.Core.IRepositories;

namespace wx.Application.Security.Users;

public record CreateUserCommand(string Account, string Password) : IRequest<Unit>;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(ILogger<CreateUserCommandValidator> logger)
    {
        RuleFor(command => command.Account).NotEmpty();
        RuleFor(command => command.Password).NotEmpty();
    }
}

public class CreateUserCommandHandler(IUserRepository repository, IPasswordHasher passwordHasher) : IRequestHandler<CreateUserCommand, Unit>
{
    public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Account))
        {
            throw new BadRequestException("Account cannot be empty");
        }
        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new BadRequestException("Password cannot be empty");
        }
            
        var userExists = await repository.GetAsync(u => u.Account == request.Account);

        if (userExists?.Any() == true) throw new ConflictException("User already exists");

        var password = passwordHasher.Hash(request.Password);
        var user = User.Create(request.Account, password);
        await repository.AddAsync(user, cancellationToken);
        return Unit.Value;
    }

}
