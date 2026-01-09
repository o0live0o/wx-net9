using wx.Application.Common.Exceptions;
using wx.Core.Domain.Security;

namespace wx.Application.Security.Users;

public record DeleteUserCommand(string Account) : IRequest<Unit>;

public class DeleteUserCommandHander(IRepository<User> repository) : IRequestHandler<DeleteUserCommand, Unit>
{
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var users = await repository.GetAsync(p => p.Account == request.Account);

        if (users?.Any() != true)
        {
            throw new BadRequestException("User not found");
        }

        var user = users.First();
        user.Deactivate();

        await repository.UpdateAsync(user, cancellationToken);

        return Unit.Value;
    }
}