using Asp.Versioning;
using Microsoft.AspNetCore.Http.HttpResults;
using wx.Core.Entities;

namespace wx.MiniApi.Apis;

[ApiVersion(1.0)]

public static class StockApi
{
    public static RouteGroupBuilder MapStockApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/stock");

        api.MapGet("cancel", CancelOrderAsync);

        return api;
    }

    public static async Task<Results<Ok, BadRequest<string>, ProblemHttpResult>> CancelOrderAsync()
    {
        return TypedResults.Ok();
    }

}
