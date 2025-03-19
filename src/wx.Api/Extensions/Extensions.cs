using FluentValidation;
using wx.Application.Behaviors;
using wx.Application.Stock.Create;
using wx.Application.Validations;
using wx.Core.Constants;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining(typeof(CreateStockCommand));
            cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
            //cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });
        builder.Services.AddSingleton<IValidator<CreateStockCommand>, CreateStockCommandValidator>();
    }

    public static void RegisterHttpClient(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHttpClient(CoreConstant.HTTP_CLIENT_NORMAL).AddStandardResilienceHandler();
    }

    public static void AddCustomResilience(this IHostApplicationBuilder builder)
    {

    }
}
