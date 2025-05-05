using wx.Api.Extensions;
using wx.Application.Events;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using NSwag.Generation.Processors.Security;
using NSwag;
using NSwag.AspNetCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using wx.Application.Products;
using FluentValidation;
using wx.Shared.Images;
using Microsoft.Extensions.FileProviders;
using wx.Application.Categories;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddInfrastructureServices(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.AddApplicationServices();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddHttpClient();
builder.Services.AddSignalR();
builder.Services.AddHealthChecks().AddDbContextCheck<WxContext>(name:"db", tags: ["db"]);
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
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagePath),
    RequestPath = "/images",
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append(
            "Cache-Control", $"public, max-age={24 * 60 * 60}");
    }
});
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