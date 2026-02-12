using MongoDB.Bson.Serialization;
using wx.Core.Domain.Security;
using wx.Core.Domain.Stocks;

namespace wx.Infrastructure.Persistence.MongoDb;

internal static class MongoDbConfiguration
{
    public static void RegisterMappings()
    {
        BsonClassMap.RegisterClassMap<User>(cm =>
        {
            cm.AutoMap();
            //cm.MapIdMember(u => u.Id);
            //cm.UnmapProperty(u => u.Roles);
            cm.UnmapField("_roles");
            cm.SetIsRootClass(true);
            cm.SetIgnoreExtraElements(true);
        });

        BsonClassMap.RegisterClassMap<Role>(cm =>
        {
            cm.AutoMap();
            //cm.MapIdMember(u => u.Id);
            //cm.UnmapProperty(u => u.Menus);
            //cm.UnmapProperty(u => u.Permissions);
            cm.UnmapField("_menus");
            cm.UnmapField("_permissions");
            cm.SetIsRootClass(true);
            cm.SetIgnoreExtraElements(true);
        });

        BsonClassMap.RegisterClassMap<StockAccountAI>(cm =>
        {
            cm.AutoMap();
            cm.MapField("_positions").SetElementName("positions");
            cm.UnmapMember(p => p.Positions);
        });
    }
}
