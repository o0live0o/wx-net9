using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using wx.Core.SeedWork;

namespace wx.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,string connectionString)
    {
        services.AddDbContext<WxContext>(options =>
        {
            options.UseNpgsql(connectionString);
            //options.UseNpgsql(@"Host=127.0.0.1;Username=postgres;Password=123456;Database=wx01");
        });

        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        return services;
    }

    public static async Task MigrateDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<WxContext>();

        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
