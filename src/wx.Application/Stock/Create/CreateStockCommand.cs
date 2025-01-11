using MediatR;

namespace wx.Application.Stock.Create;

public record CreateStockCommand(string code):IRequest<string>;
