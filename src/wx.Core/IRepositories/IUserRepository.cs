using wx.Core.Domain.Security;

namespace wx.Core.IRepositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetUserByAccountAsync(string account);

    Task AssignRolesAsync(int userId,IEnumerable<int> roleIds,CancellationToken cancellationToken);
}