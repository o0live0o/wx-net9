using wx.Core.Domain.Security;
using wx.Core.IRepositories;
using wx.Infrastructure.Persistence.MongoDb.Documents;

namespace wx.Infrastructure.Persistence.MongoDb.Repositories;

public class RoleRepository : MongoRepository<Role>, IRoleRepository
{
    private readonly MongoDbContext _dbContext;
    private readonly IMongoCollection<RolePermissionDocument> _rolePermissionCollection;
    public RoleRepository(MongoDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
        _rolePermissionCollection = _dbContext.Database.GetCollection<RolePermissionDocument>("role_permission");
    }

    public async Task<IEnumerable<Role>> GetRoleByIds(int[] ids)
    {
        FilterDefinition<Role> filter = Builders<Role>.Filter.In(r => r.Id, ids);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task AssignPermissionsAsync(int roleId, IEnumerable<int> permissionIds, CancellationToken cancellationToken)
    {
        using var session = await _dbContext.Database.Client.StartSessionAsync();
        session.StartTransaction();
        try
        {
            var existingPermissions = await _rolePermissionCollection
                   .Find(ur => ur.RoleId == roleId)
                   .ToListAsync(cancellationToken);

            var currentPermissions = existingPermissions.Select(r => r.PermissionId).ToList();
            var toAdd = permissionIds.Except(currentPermissions).ToList();
            var toRemove = currentPermissions.Except(permissionIds).ToList();

            if (toRemove.Any())
            {
                var deleteFilter = Builders<RolePermissionDocument>.Filter
                    .Where(ur => toRemove.Contains(ur.PermissionId) && ur.RoleId == roleId);
                await _rolePermissionCollection.DeleteManyAsync(deleteFilter, cancellationToken);
            }

            if (toAdd.Any())
            {
                var insertDocs = toAdd.Select(permissionId =>
                    new RolePermissionDocument { RoleId  = roleId, PermissionId = permissionId });
                await _rolePermissionCollection.InsertManyAsync(insertDocs);
            }

            await session.CommitTransactionAsync();
        }
        catch
        {
            await session.AbortTransactionAsync(cancellationToken);
            throw;
        }
    }
}
