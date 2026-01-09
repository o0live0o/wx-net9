using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using wx.Application.Common.Interfaces;

namespace wx.Api.Endpoints;

public static class IdentityApi
{
    public static RouteGroupBuilder MapIdentityApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/identity");
        api.WithTags("Identity");
        api.MapPost("login", LoginAsync);
        return api;
    }

    public static async Task<Results<Ok<TokenDto>, BadRequest>> LoginAsync([FromBody] LoginRequest request, [FromServices] IIdentityService identityService, HttpContext httpContext)
    {
        var deviceId = request.DeviceId ?? Guid.NewGuid().ToString();
        var remoteIp = httpContext.Connection.RemoteIpAddress;
        if (remoteIp?.IsIPv4MappedToIPv6 == true)
            remoteIp = remoteIp.MapToIPv4();
        var token = await identityService.GetTokenAsync(request.Username, request.Password, remoteIp.ToString(), deviceId);
        return TypedResults.Ok(token);
    }
}

public record LoginRequest(string Username, string Password, string? DeviceId);