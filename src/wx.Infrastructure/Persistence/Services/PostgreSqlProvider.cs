using Microsoft.EntityFrameworkCore;
using wx.Application.Common.Services;

namespace wx.Infrastructure.Persistence.Services;

public class PostgreSqlProvider: IDatabaseProvider
{
    public DbContextOptions<TContext> Configure<TContext>(DbContextOptionsBuilder<TContext> optionsBuilder) where TContext : DbContext
    {
        throw new NotImplementedException();
    }
}




