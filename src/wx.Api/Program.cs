using wx.Api.Extensions;
using wx.Application.Events;
using Serilog;
using wx.Application.Fearures.Categories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using NSwag.Generation.Processors.Security;
using NSwag;
using NSwag.AspNetCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddOpenApiDocument(
  doc =>
  {
      doc.DocumentName = "TodoAPI";
      doc.Title = "TodoAPI v1";
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
                      { "api://f5d043aa-2000-4ded-9913-955039eed877/Files.Read", "Read access to protected resources" },
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
builder.Services.AddInfrastructureServices(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.AddApplicationServices();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddScoped<ICategoryQuery, CategoryQuery>();
builder.Services.AddSingleton<IEventBus, EventBus>();
builder.Services.AddSingleton<InMemoryMessageQueue>();
builder.Services.AddHostedService<IntergrationEventProcessorJob>();
builder.Services.AddHttpClient();
builder.Services.AddSignalR();
builder.Services.AddHealthChecks();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(options =>
{
    builder.Configuration.Bind("AzureAd", options);

}, options =>
{
    builder.Configuration.Bind("AzureAd", options);
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigin",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});
var app = builder.Build();
await app.Services.MigrateDatabaseAsync();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseOpenApi();

    app.UseSwaggerUi(settings =>
    {
        settings.OAuth2Client = new OAuth2ClientSettings();
        settings.OAuth2Client.ClientId = "a7c66bf1-300f-462a-8258-f57414f21ec2";
    });
}

app.UseCors("AllowAllOrigin");
app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                duration = e.Value.Duration
            })
        });
    }
});
app.MapControllers();

app.Run();