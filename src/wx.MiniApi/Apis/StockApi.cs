using Asp.Versioning;
using Microsoft.AspNetCore.Http.HttpResults;
using wx.Application.Queries;
using wx.Core.Entities;

namespace wx.MiniApi.Apis;

[ApiVersion(1.0)]

public static class StockApi
{
    public static RouteGroupBuilder MapStockApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/stock");

        api.MapGet("cancel", CancelOrderAsync);
        api.MapGet("{code}", QueryStockByCodeAsync);


        return api;
    }

    public static async Task<Results<Ok, BadRequest<string>, ProblemHttpResult>> CancelOrderAsync()
    {
        return TypedResults.Ok();
    }

    public static async Task<Results<Ok<Stock>, BadRequest<string>, ProblemHttpResult>> QueryStockByCodeAsync(IStockQuery stockQuery,string code)
    {
        var stock = await stockQuery.GetStockByCodeAsync(code);
        return TypedResults.Ok(stock);
    }
}
