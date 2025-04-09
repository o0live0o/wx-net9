using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using wx.Core.Entities;
using wx.Core.SeedWork;

namespace wx.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<WxContext>(options =>
        {
            var connection = new SqliteConnection(
                new SqliteConnectionStringBuilder(connectionString)
                {
                    ForeignKeys = true  
                }.ToString());

            options.UseSqlite(connection, sqliteOptions => {
                sqliteOptions.CommandTimeout(30);
                sqliteOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                sqliteOptions.UseRelationalNulls();

            });
            
            #if DEBUG
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
            options.LogTo(Console.WriteLine);
            #endif
            // options.UseNpgsql(connectionString, sqlOptions =>
            // {
            //     sqlOptions.EnableRetryOnFailure(
            //         maxRetryCount: 3,
            //         maxRetryDelay: TimeSpan.FromSeconds(30),
            //         errorCodesToAdd: null
            //     );
            // });
            //options.UseNpgsql(@"Host=127.0.0.1;Username=postgres;Password=123456;Database=wx01");
        });

        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        return services;
    }

    public static async Task MigrateDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WxContext>();

        try
        {
            await dbContext.Database.MigrateAsync();

            if (!await dbContext.Categories.AnyAsync(c => c.Id == 1))
            {
                dbContext.Categories.Add(new Category("Root", null));
                await dbContext.SaveChangesAsync();
            }
        }
        catch
        {
            throw;
        }
    }
}
