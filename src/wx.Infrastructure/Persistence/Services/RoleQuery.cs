using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using wx.Application.Security.Roles;
using wx.Core.Domain.Security;

namespace wx.Infrastructure.Persistence.Services;

[AutoRegister]
public class RoleQuery(IMongoDatabase database) : IRoleQuery
{

    public Task<IEnumerable<Role>> GetRolesAsync()
    {
        var collection = database.GetCollection<Role>("role");
        throw new NotImplementedException();
    }
}
