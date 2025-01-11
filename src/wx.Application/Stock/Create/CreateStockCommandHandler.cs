using MediatR;
using wx.Infrastructure;

namespace wx.Application.Stock.Create;

public sealed class CreateStockCommandHandler : IRequestHandler<CreateStockCommand,string>
{
    private readonly WxContext dbContext;

    public CreateStockCommandHandler(WxContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<string> Handle(CreateStockCommand request, CancellationToken cancellationToken)
    {
        await Task.Delay(10);
        return "Requst Ok";
    }
}
