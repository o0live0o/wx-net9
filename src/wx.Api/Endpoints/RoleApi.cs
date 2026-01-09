using Microsoft.AspNetCore.Mvc;

namespace wx.Api.Endpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using wx.Application.Security.Roles;

public static class RoleApi
{
    public static RouteGroupBuilder MapRoleApi(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/roles");
        group.WithTags("Role");
        group.MapGet("/", GetRoleAsync);
        group.MapPost("/", AddRoleAsync);
        group.MapDelete("/{roleId:int}", DeleteRoleAsync);
        return group;
    }

    public static async Task<Ok<string>> GetRoleAsync()
    {
        await Task.CompletedTask;
        return TypedResults.Ok("1");
    }

    public static async Task<Results<NoContent, BadRequest, Conflict>> AddRoleAsync([FromBody] AddRoldRequest request,
        [FromServices] IMediator mediator)
    {
        var cmd = new CreateRoleCommand(request.Name, request.Code);
        await mediator.Send(cmd);
        return TypedResults.NoContent();
    }

    public static async Task<Results<NoContent, BadRequest>> DeleteRoleAsync([FromRoute] int roleId,
        [FromServices] IMediator mediator)
    {
        var cmd = new DeactiveRoleCommand(roleId);
        await mediator.Send(cmd);
        return TypedResults.NoContent();
    }
}

public record AddRoldRequest(string Code, string Name);