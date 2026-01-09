using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Reflection;
using wx.Application.Common.Interfaces;
using wx.Application.Security.Users;
using wx.Core.Entities;
using wx.Core.IRepositories;
using wx.Core.SeedWork;
using wx.Infrastructure.Options;
using wx.Infrastructure.Persistence.EFCore;
using wx.Infrastructure.Persistence.MongoDb;
using wx.Infrastructure.Persistence.MongoDb.Repositories;
using wx.Infrastructure.Persistence.Services;
using wx.Infrastructure.Security;

namespace wx.Infrastructure.Extensions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Scan(scan => scan.FromAssemblies(Assembly.GetExecutingAssembly())
                                                        .AddClasses(classes => classes.WithAttribute<AutoRegisterAttribute>())
                                                        .AsImplementedInterfaces()
                                                        .UsingAttributes()
        );
        services.ConfigDb(configuration);
        services.Configure<TokenOptions>(configuration.GetSection(TokenOptions.SectionName));

        return services;
    }

    private static void ConfigDb(this IServiceCollection services, IConfiguration configuration)
    {
        var dbProvider = configuration["DatabaseProvider"];
        var connectionString = configuration.GetConnectionString(dbProvider);

        switch (dbProvider.ToLowerInvariant())
        {
            case "mongodb":
                ConfigureMongoDB(services, configuration);
                break;
            case "sqlite":
                ConfigSqlite(services, configuration);
                break; 
        }
    }

    private static void ConfigSqlite(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<WxContext>(options =>
        {
            var connection = new SqliteConnection(
                new SqliteConnectionStringBuilder(@"Data Source=E:/project/data/main.db")
                {
                    ForeignKeys = true
                }.ToString());

            options.UseSqlite(connection, sqliteOptions =>
            {
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
    }

    private static void ConfigureMongoDB(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDB");
        var databaseName = new MongoUrl(connectionString).DatabaseName;
        databaseName = "wx";

        services.AddSingleton<IMongoClient>(_ => new MongoClient(connectionString));
        services.AddScoped(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(databaseName);
        });

        MongoDbConfiguration.RegisterMappings();
        services.AddScoped<MongoDbContext>();
        services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));
    }

    public static async Task MigrateDatabaseAsync(this IServiceProvider serviceProvider)
    {
        var dbProvider = serviceProvider.GetRequiredService<IConfiguration>()["DatabaseProvider"];

        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WxContext>();
        if (dbProvider!.ToLowerInvariant() == "mongodb")
        {

        }
        try
        {
            //await dbContext.Database.MigrateAsync();

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

    public static IServiceCollection AddService(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes()
            .Where(t => t.GetCustomAttribute<AutoRegisterAttribute>() != null);

        foreach (var type in types)
        {
            var attr = type.GetCustomAttribute<AutoRegisterAttribute>()!;

            foreach (var @interface in type.GetInterfaces())
            {
                services.Add(new ServiceDescriptor(@interface, type, attr.Lifetime));
            }

            if (attr.AsSelf)
            {
                services.Add(new ServiceDescriptor(type, type, attr.Lifetime));
            }
        }

        return services;
    }
}