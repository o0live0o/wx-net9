using wx.Core.Domain.Security;

namespace wx.Application.Security.Roles;

public interface IRoleQuery
{
    Task<IEnumerable<Role>> GetRolesAsync();
}