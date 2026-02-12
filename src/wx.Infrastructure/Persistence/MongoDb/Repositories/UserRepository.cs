using wx.Core.Domain.Security;
using wx.Core.IRepositories;
using wx.Infrastructure.Persistence.MongoDb.Documents;

namespace wx.Infrastructure.Persistence.MongoDb.Repositories;

public class UserRepository : MongoRepository<User>, IUserRepository
{
    private readonly MongoDbContext _dbContext;

    private readonly IMongoCollection<UserRoleDocument> _userRoleCollection;
    private readonly IMongoCollection<Role> _roleCollection;
    public UserRepository(MongoDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
        _userRoleCollection = _dbContext.Database.GetCollection<UserRoleDocument>("user_roles");
        _roleCollection = _dbContext.GetCollection<Role>();

    }

    public async Task<User?> GetUserByAccountAsync(string account)
    {
        if (string.IsNullOrWhiteSpace(account))
            return null;

        FilterDefinition<User> filter = Builders<User>.Filter.Eq(u => u.Account, account);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
    
    public override async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        using var session = await _dbContext.Database.Client.StartSessionAsync();
        session.StartTransaction();
        try
        {
            await _collection.ReplaceOneAsync(
                    u => u.Id == user.Id,
                    user,
                    cancellationToken: cancellationToken);

            await session.CommitTransactionAsync();
        }
        catch
        {
            await session.AbortTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task AssignRolesAsync(int userId, IEnumerable<int> roleIds, CancellationToken cancellationToken)
    {
        using var session = await _dbContext.Database.Client.StartSessionAsync();
        session.StartTransaction();
        try
        {
            var existingRoles = await _userRoleCollection
                   .Find(ur => ur.UserId == userId)
                   .ToListAsync(cancellationToken);

            var currentRoleIds = existingRoles.Select(r => r.RoleId).ToList();
            var toAdd = roleIds.Except(currentRoleIds).ToList();
            var toRemove = currentRoleIds.Except(roleIds).ToList();

            if (toRemove.Any())
            {
                var deleteFilter = Builders<UserRoleDocument>.Filter
                    .Where(ur => toRemove.Contains(ur.RoleId) && ur.UserId == userId);

                await _userRoleCollection.DeleteManyAsync(deleteFilter, cancellationToken);
            }

            if (toAdd.Any())
            {
                var insertDocs = toAdd.Select(roleId =>
                    new UserRoleDocument { UserId = userId, RoleId = roleId });

                await _userRoleCollection.InsertManyAsync(insertDocs);
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

//var pipeline = new List<BsonDocument>();

//// 第1阶段：匹配用户
////$match 已确保最多1个用户
//pipeline.Add(new BsonDocument("$match", new BsonDocument("Account", account)));

//// 第2阶段：关联用户角色表
//pipeline.Add(new BsonDocument("$lookup", new BsonDocument
//{
//    {"from", "user_roles"},
//    {"localField", "_id"},
//    {"foreignField", "UserId"},
//    {"as", "userRoleInfo"}
//}));


//// 第3阶段：从 userRoleInfo 数组中提取所有 RoleId（用于下一步 lookup）
//pipeline.Add(new BsonDocument("$addFields", new BsonDocument
//{
//    {"roleIds", "$userRoleInfo.RoleId"} // → [5, 7]
//}));

//// 第4阶段：一次性关联所有角色（通过 roleIds 数组）
//pipeline.Add(new BsonDocument("$lookup", new BsonDocument
//{
//    {"from", "Role"},
//    {"localField", "roleIds"},      // ← 关键：用数组匹配
//    {"foreignField", "_id"},
//    {"as", "roleInfo"}              // → [ { _id:5, ... }, { _id:7, ... } ]
//}));

//// 第5阶段：移除中间字段（可选）
//pipeline.Add(new BsonDocument("$project", new BsonDocument
//{
//    {"userRoleInfo", 0},
//    {"roleIds", 0}
//}));



//try
//{
//    var results = await _collection.Aggregate<BsonDocument>(pipeline).ToListAsync();
//    var user = BsonSerializer.Deserialize<User>(results[0]);
//    if (user != null && results[0].TryGetValue("roleInfo", out var roleInfoElement) && roleInfoElement.IsBsonArray)
//    {

//        foreach (var item in roleInfoElement.AsBsonArray)
//        {
//            var role = BsonSerializer.Deserialize<Role>(item.AsBsonDocument);
//            user.AssignRole(role);
//        }
//    }

//    return user;
//}
//catch (Exception ex)
//{
//    // 记录异常日志
//    Console.WriteLine($"聚合管道查询失败: {ex.Message}");
//    throw;
//}
//public async Task<User?> GetUserByAccountAsync(string account)
//{
//    var pipeline = new List<IPipelineStageDefinition>();

//    // 第1阶段：匹配用户
//    pipeline.Add(PipelineStageDefinitionBuilder.Match(
//        Builders<User>.Filter.Eq(u => u.Account, account)
//    ));
//    var localField = new ExpressionFieldDefinition<User, int>(u => u.Id);
//    var foreignField = new ExpressionFieldDefinition<UserRoleDocument, int>(ur => ur.UserId);
//    // 第2阶段：关联用户角色表
//    pipeline.Add(PipelineStageDefinitionBuilder.Lookup<User, UserRoleDocument, BsonDocument>(
//        _userRoleCollection,
//       localField,
//       foreignField,
//        "user_roles"
//    ));

//    // 第3阶段：过滤有角色的用户
//    pipeline.Add(PipelineStageDefinitionBuilder.AddFields(new BsonDocument("$addFields", new BsonDocument
//    {
//        {"hasRole", new BsonDocument("$gt", new BsonArray { new BsonDocument("$size", new BsonDocument("$ifNull", new BsonArray {"$userRoleInfo", new BsonArray()})), 0 })}
//    })));

//    // 第4阶段：展开用户角色信息
//    pipeline.Add(PipelineStageDefinitionBuilder.Unwind(new BsonDocument("$unwind", new BsonDocument
//    {
//        {"path", "$userRoleInfo"},
//        {"preserveNullAndEmptyArrays", false}
//    })));

//    // 第5阶段：关联角色信息
//    pipeline.Add(PipelineStageDefinitionBuilder.Lookup<BsonDocument, RoleDocument, BsonDocument>(
//        _roleCollection,
//        "userRoleInfo.RoleId",
//        r => r.Id,
//        "roleInfo"
//    ));

//    // 第6阶段：展开角色信息
//    pipeline.Add(PipelineStageDefinitionBuilder.Unwind("roleInfo"));

//    // 第7阶段：添加菜单ID字段（使用子查询方式避免笛卡尔积）
//    pipeline.Add(PipelineStageDefinitionBuilder.AddFields(new BsonDocument("$addFields", new BsonDocument
//    {
//        {"menuIds", new BsonDocument("$let", new BsonDocument
//        {
//            {"vars", new BsonDocument("roleId", "$roleInfo._id")},
//            {"in", new BsonDocument("$map", new BsonDocument
//            {
//                {"input", new BsonDocument("$filter", new BsonDocument
//                {
//                    {"input", new BsonDocument("$list", new BsonArray {_roleMenuCollection.Alias})},
//                    {"cond", new BsonDocument("$eq", new BsonArray {"$$this.RoleId", "$$roleId"})}
//                })},
//                {"as", "item"},
//                {"in", "$$item.MenuId"}
//            })}
//        })}
//    })));

//    // 第8阶段：添加权限ID字段（使用子查询方式避免笛卡尔积）
//    pipeline.Add(PipelineStageDefinitionBuilder.AddFields(new BsonDocument("$addFields", new BsonDocument
//    {
//        {"permissionIds", new BsonDocument("$let", new BsonDocument
//        {
//            {"vars", new BsonDocument("roleId", "$roleInfo._id")},
//            {"in", new BsonDocument("$map", new BsonDocument
//            {
//                {"input", new BsonDocument("$filter", new BsonDocument
//                {
//                    {"input", new BsonDocument("$list", new BsonArray {_rolePermissionCollection.Alias})},
//                    {"cond", new BsonDocument("$eq", new BsonArray {"$$this.RoleId", "$$roleId"})}
//                })},
//                {"as", "item"},
//                {"in", "$$item.PermissionId"}
//            })}
//        })}
//    })));

//    // 第9阶段：根据菜单ID获取完整菜单信息
//    pipeline.Add(PipelineStageDefinitionBuilder.AddFields(new BsonDocument("$addFields", new BsonDocument
//    {
//        {"menus", new BsonDocument("$let", new BsonDocument
//        {
//            {"vars", new BsonDocument("menuIds", "$menuIds")},
//            {"in", new BsonDocument("$filter", new BsonDocument
//            {
//                {"input", new BsonDocument("$list", new BsonArray {_menuCollection.Alias})},
//                {"cond", new BsonDocument("$in", new BsonArray {"$$this._id", "$$menuIds"})}
//            })}
//        })}
//    })));

//    // 第10阶段：根据权限ID获取完整权限信息
//    pipeline.Add(PipelineStageDefinitionBuilder.AddFields(new BsonDocument("$addFields", new BsonDocument
//    {
//        {"permissions", new BsonDocument("$let", new BsonDocument
//        {
//            {"vars", new BsonDocument("permissionIds", "$permissionIds")},
//            {"in", new BsonDocument("$filter", new BsonDocument
//            {
//                {"input", new BsonDocument("$list", new BsonArray {_permissionCollection.Alias})},
//                {"cond", new BsonDocument("$in", new BsonArray {"$$this._id", "$$permissionIds"})}
//            })}
//        })}
//    })));

//    // 第11阶段：限制返回结果数量
//    pipeline.Add(PipelineStageDefinitionBuilder.Limit(1));

//    try
//    {
//        var results = await _userCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();

//        if (results == null || results.Count == 0)
//        {
//            // 检查用户是否存在但没有角色
//            var userFilter = Builders<UserDocument>.Filter.Eq(u => u.Account, account);
//            var userDoc = await _userCollection.Find(userFilter).FirstOrDefaultAsync();

//            if (userDoc != null)
//            {
//                return new User
//                {
//                    Id = userDoc.Id,
//                    Account = userDoc.Account,
//                    Name = userDoc.Name,
//                    CreatedAt = userDoc.CreatedAt,
//                    IsActive = userDoc.IsActive,
//                    Role = null
//                };
//            }

//            return null;
//        }

//        var result = results[0];

//        // 构建用户对象
//        var user = new User
//        {
//            Id = result["_id"].AsObjectId,
//            Account = result["Account"].AsString,
//            Name = result["Name"].AsString,
//            CreatedAt = result["CreatedAt"].ToUniversalTime(),
//            IsActive = result["IsActive"].ToBoolean()
//        };

//        // 构建角色对象（如果存在）
//        if (result.Contains("roleInfo") && !result["roleInfo"].IsBsonNull)
//        {
//            var roleBson = result["roleInfo"].AsBsonDocument;
//            var role = new Role
//            {
//                Id = roleBson["_id"].AsObjectId,
//                Name = roleBson["Name"].AsString,
//                Code = roleBson["Code"].AsString,
//                Description = roleBson["Description"].AsString,
//                IsActive = roleBson["IsActive"].ToBoolean()
//            };

//            // 添加菜单信息
//            if (result.Contains("menus") && result["menus"].IsBsonArray)
//            {
//                var menusArray = result["menus"].AsBsonArray;
//                foreach (var menuBsonValue in menusArray)
//                {
//                    if (menuBsonValue.IsBsonDocument)
//                    {
//                        var menuBson = menuBsonValue.AsBsonDocument;
//                        role.Menus.Add(new Menu
//                        {
//                            Id = menuBson["_id"].AsObjectId,
//                            Name = menuBson["Name"].AsString,
//                            Url = menuBson["Url"]?.AsString ?? "",
//                            Icon = menuBson["Icon"]?.AsString ?? "",
//                            Order = menuBson["Order"].ToInt32(),
//                            ParentId = menuBson["ParentId"]?.AsString ?? "",
//                            IsActive = menuBson["IsActive"].ToBoolean()
//                        });
//                    }
//                }
//            }

//            // 添加权限信息
//            if (result.Contains("permissions") && result["permissions"].IsBsonArray)
//            {
//                var permissionsArray = result["permissions"].AsBsonArray;
//                foreach (var permissionBsonValue in permissionsArray)
//                {
//                    if (permissionBsonValue.IsBsonDocument)
//                    {
//                        var permissionBson = permissionBsonValue.AsBsonDocument;
//                        role.Permissions.Add(new Permission
//                        {
//                            Id = permissionBson["_id"].AsObjectId,
//                            Name = permissionBson["Name"].AsString,
//                            Code = permissionBson["Code"].AsString,
//                            Description = permissionBson["Description"].AsString,
//                            IsActive = permissionBson["IsActive"].ToBoolean()
//                        });
//                    }
//                }
//            }

//            user.Role = role;
//        }

//        return user;
//    }
//    catch (Exception ex)
//    {
//        // 记录异常日志
//        Console.WriteLine($"聚合管道查询失败: {ex.Message}");
//        throw;
//    }
//}

