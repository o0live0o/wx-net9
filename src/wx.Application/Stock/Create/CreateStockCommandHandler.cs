using MediatR;
using Microsoft.Extensions.Logging;
using wx.Infrastructure;

namespace wx.Application.Stock.Create;

public sealed class CreateStockCommandHandler : IRequestHandler<CreateStockCommand,string>
{
    private readonly WxContext dbContext;
    private readonly ILogger<CreateStockCommandHandler> _logger;
    public CreateStockCommandHandler(WxContext dbContext, ILogger<CreateStockCommandHandler> logger)
    {
        this.dbContext = dbContext;
        _logger = logger;
    }

    public async Task<string> Handle(CreateStockCommand request, CancellationToken cancellationToken)
    {
        ApiTrace.LogRequest(_logger,100,"Hello World");
        await Task.Delay(10);
        return "Requst Ok";
    }
}
