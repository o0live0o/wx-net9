using System;
using System.Collections.Generic;
using System.Text;
using wx.Core.Domain.Security;

namespace wx.Infrastructure.Persistence.MongoDb;

public class CollectionNameResolver
{
    public static string GetCollectionName<T>()
    {
        return GetCollectionName(typeof(T));
    }
    public static string GetCollectionName(Type entityType)
    {
        var name = entityType.Name;

        return name switch
        {
            nameof(User) => "User",
            _ => name
        };
    }
}
