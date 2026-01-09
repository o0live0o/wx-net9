using MongoDB.Driver;
using wx.Application.Security.Users;
using wx.Core.Domain.Security;
using wx.Infrastructure.Persistence.MongoDb;
using wx.Infrastructure.Services;

namespace wx.Infrastructure.Persistence.Services;

[AutoRegister]
public class UserQuery : IUserQuery
{
    private readonly IMongoDatabase _database;
    public UserQuery(IMongoDatabase database)
    {
        _database = database;
    }
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var collection = _database.GetCollection<User>(CollectionNameResolver.GetCollectionName(typeof(User)));
        var users = (await collection.FindAsync(_ => true)).ToList();
        return users.Select(user => new UserDto(user.Account));
    }
}