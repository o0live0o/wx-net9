using System;
using System.Collections.Generic;
using System.Text;
using wx.Core.Domain.Security;
using wx.Core.IRepositories;
using wx.Core.SeedWork;

namespace wx.Infrastructure.Persistence.MongoDb.Repositories;

public class PermissionRepository : MongoRepository<Permission>, IPermissionRepository
{
    public PermissionRepository(MongoDbContext dbContext) : base(dbContext)
    {
    }
}
