using MediatR;
using Microsoft.Extensions.Logging;

namespace wx.Application.Stock.Create;

public sealed class CreateStockCommandHandler : IRequestHandler<CreateStockCommand,string>
{
    private readonly ILogger<CreateStockCommandHandler> _logger;
    public CreateStockCommandHandler(ILogger<CreateStockCommandHandler> logger)
    {
        _logger = logger;
    }

    public async Task<string> Handle(CreateStockCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
