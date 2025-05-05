using FluentValidation;
using NSwag.Generation.Processors.Security;
using NSwag;
using wx.Application.Behaviors;
using wx.Application.Categories;
using wx.Application.Events;
using wx.Application.Products;
using wx.Application.Stock;
using wx.Application.Stock.Create;
using wx.Core.Constants;
using wx.Shared.Images;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.ConfigOpenApiDoc();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining(typeof(CreateStockCommand));
            cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
        });
        builder.Services.AddSingleton<IValidator<CreateStockCommand>, CreateStockCommandValidator>();
        builder.Services.AddSingleton<IValidator<CreateProductCommand>, CreateProductCommandValidator>();

        builder.Services.AddScoped<IFileService, FileService>();
        builder.Services.AddSingleton<IEventBus, EventBus>();
        builder.Services.AddSingleton<InMemoryMessageQueue>();
        builder.Services.AddHostedService<IntergrationEventProcessorJob>();
        builder.AddQueryService();

    }

    public static void AddQueryService(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<ICategoryQuery, CategoryQuery>();
        builder.Services.AddScoped<IProductQuery, ProductQuery>();
    }

    public static void RegisterHttpClient(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHttpClient(CoreConstant.HTTP_CLIENT_NORMAL).AddStandardResilienceHandler();
    }

    public static void ConfigOpenApiDoc(this IHostApplicationBuilder builder)
    {
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddOpenApiDocument(
          doc =>
          {
              doc.DocumentName = "API";
              doc.Title = "API v1";
              doc.Version = "v1";
              doc.AddSecurity("bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme
              {
                  Type = OpenApiSecuritySchemeType.OAuth2,
                  Description = "My Authentication",
                  Flow = OpenApiOAuth2Flow.Implicit,

                  Flows = new OpenApiOAuthFlows()
                  {
                      Implicit = new OpenApiOAuthFlow()
                      {
                          Scopes = new Dictionary<string, string>
                            {
                                { "api://a7c66bf1-300f-462a-8258-f57414f21ec2/web", "Write access to protected resources" }
                            },
                          AuthorizationUrl = "https://login.microsoftonline.com/f9ffb96b-5c70-45eb-884f-caeadd571298/oauth2/v2.0/authorize",
                          TokenUrl = "https://login.microsoftonline.com/f9ffb96b-5c70-45eb-884f-caeadd571298/oauth2/v2.0/token"

                      },
                  }
              });
              doc.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("bearer"));
          }
        );
    }

    public static void AddCustomResilience(this IHostApplicationBuilder builder)
    {

    }
}
