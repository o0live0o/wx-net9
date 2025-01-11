using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using wx.Core.SeedWork;

namespace wx.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddDbContext<WxContext>(options =>
        {
            options.UseNpgsql(@"Host=127.0.0.1;Username=postgres;Password=123456;Database=wx01");
        });

        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        return services;
    }
}
