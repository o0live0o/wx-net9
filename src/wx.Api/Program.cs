using System.Net;
using System.Reflection;
using wx.Api.Extensions;
using wx.Application.Events;
using Serilog;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

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
builder.Services.AddInfrastructureServices();
builder.AddApplicationServices();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddSingleton<IEventBus, EventBus>();
builder.Services.AddSingleton<InMemoryMessageQueue>();
builder.Services.AddHostedService<IntergrationEventProcessorJob>();
builder.Services.AddHttpClient();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigin",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAllOrigin");
app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseAuthorization();
app.UseExceptionHandler();
app.MapControllers();

app.Run();