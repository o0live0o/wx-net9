using FluentValidation;
using NSwag;
using NSwag.Generation.Processors.Security;
using Polly;
using Polly.Retry;
using System.Net;
using wx.Application.Behaviors;
using wx.Application.Common.Constants;
using wx.Application.Stock.Create;
using wx.Core.Domain.Events;
using wx.Infrastructure.BackgroundJobs;
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
        // builder.Services.AddSingleton<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
        builder.RegisterValidators();

        builder.Services.AddScoped<IFileService, FileService>();
        builder.Services.AddSingleton<IEventBus, EventBus>();
        builder.Services.AddSingleton<InMemoryMessageQueue>();
        builder.Services.AddHostedService<IntergrationEventProcessorJob>();
        builder.AddQueryService();

    }

    private static ValueTask<bool> HandleTransientHttpError(Outcome<HttpResponseMessage> outcome) => outcome switch
    {
        { Exception: HttpRequestException } => PredicateResult.True(),
        { Result.StatusCode: HttpStatusCode.RequestTimeout } => PredicateResult.True(),
        { Result.StatusCode: >= HttpStatusCode.InternalServerError } => PredicateResult.True(),
        _ => PredicateResult.False()
    };

    private static RetryStrategyOptions<HttpResponseMessage> GetRetryOptions() =>
    new()
    {
        ShouldHandle = args => HandleTransientHttpError(args.Outcome),
        MaxRetryAttempts = 3,
        BackoffType = DelayBackoffType.Exponential,
        Delay = TimeSpan.FromSeconds(2)
    };

    public static void AddQueryService(this IHostApplicationBuilder builder)
    {

    }

    public static void RegisterHttpClient(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHttpClient(HttpConstant.HTTP_CLIENT_NORMAL).AddStandardResilienceHandler();
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

    public static void RegisterValidators(this IHostApplicationBuilder builder)
    {
        var assemblies = new[]
        {
            typeof(CreateStockCommand).Assembly, 
        };

        foreach (var assembly in assemblies)
        {
            var validatorTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>))
                    .Select(i => new { ValidatorType = t, ValidatedType = i.GetGenericArguments()[0] }))
                .ToList();

            foreach (var validator in validatorTypes)
            {
                var serviceType = typeof(IValidator<>).MakeGenericType(validator.ValidatedType);
                builder.Services.AddSingleton(serviceType, validator.ValidatorType);
            }
        }
    }
}
