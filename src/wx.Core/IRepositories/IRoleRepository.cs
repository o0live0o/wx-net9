using wx.Core.Domain.Security;

namespace wx.Core.IRepositories;

public interface IRoleRepository : IRepository<Role>
{
    Task<IEnumerable<Role>> GetRoleByIds(int[] id);

    Task AssignPermissionsAsync(int roleId, IEnumerable<int> permissionIds, CancellationToken cancellationToken);
}
