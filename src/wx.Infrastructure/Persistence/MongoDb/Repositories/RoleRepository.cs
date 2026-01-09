using wx.Core.Domain.Security;
using wx.Core.IRepositories;

namespace wx.Infrastructure.Persistence.MongoDb.Repositories;

public class RoleRepository : MongoRepository<Role>, IRoleRepository
{
    public RoleRepository(MongoDbContext dbContext) : base(dbContext)
    {
    }

    public Task<IEnumerable<Role>> GetRoleByIds(int[] id)
    {
        throw new NotImplementedException();
    }
}
