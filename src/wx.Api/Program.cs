using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using NSwag.AspNetCore;
using Polly;
using Polly.Retry;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using wx.Api.Endpoints;
using wx.Api.Extensions;
using wx.Application.Common.Interfaces;
using wx.Application.Common.Services;
using wx.Infrastructure.Extensions;
using wx.Infrastructure.Options;
using wx.Infrastructure.Persistence.EFCore;
using wx.Infrastructure.Security;
using wx.Infrastructure.Services.Stock;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.AddApplicationServices();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddHttpClient();
builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddHealthChecks().AddDbContextCheck<WxContext>(name: "db", tags: ["db"]);

builder.Services.AddHttpClient<SinaProvider>().AddResilienceHandler("sina", resilience => {
    resilience.AddTimeout(TimeSpan.FromSeconds(15));
    resilience.AddRetry(new Polly.Retry.RetryStrategyOptions<HttpResponseMessage> {
        MaxRetryAttempts = 5,
        Delay = TimeSpan.FromSeconds(5),
        BackoffType = DelayBackoffType.Exponential,
        UseJitter = true, // 防止惊群效应
        ShouldHandle = static cfg =>
        {
            var ex = cfg.Outcome.Exception;
            if (ex is HttpRequestException || ex is OperationCanceledException)
                return new(true);

            var response = cfg.Outcome.Result;
            if (response is not null && !response.IsSuccessStatusCode)
                return new(true);

            return new(false);
        } 
    });

    //// 可选：加熔断器
    //resilience.AddCircuitBreaker(new()
    //{
    //    FailureThreshold = 0.5,      // 50% 失败就熔断
    //    SamplingDuration = TimeSpan.FromSeconds(30),
    //    MinimumThroughput = 10,      // 至少 10 次调用才判断
    //    BreakDuration = TimeSpan.FromMinutes(1) // 熔断 1 分钟
    //});
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var tokenOptions = builder.Configuration.GetSection(TokenOptions.SectionName).Get<TokenOptions>();

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = tokenOptions!.Issuer,
        ValidAudience = tokenOptions!.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(tokenOptions.SecretKey))
    };

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var token = context.SecurityToken as JwtSecurityToken;
            var tokenId = token?.Claims.FirstOrDefault(c => c.Type == "jti")?.Value;
        }
    };
});

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(options =>
//{
//    builder.Configuration.Bind("AzureAd", options);
//}, options =>
//{
//    builder.Configuration.Bind("AzureAd", options);
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigin",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

var app = builder.Build();
// await app.Services.MigrateDatabaseAsync();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
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
var imagePath = app.Configuration.GetValue<string>("FileStorage:ImagePath");
app.UseStaticFiles(
// new StaticFileOptions
// {
//     FileProvider = new PhysicalFileProvider(imagePath),
//     RequestPath = "/images",
//     OnPrepareResponse = ctx =>
//     {
//         ctx.Context.Response.Headers.Append(
//             "Cache-Control", $"public, max-age={24 * 60 * 60}");
//     }
// }
);
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

app.MapUsersApi();
app.MapRoleApi();
app.MapPermissionApi();
app.MapIdentityApi();

app.Run();