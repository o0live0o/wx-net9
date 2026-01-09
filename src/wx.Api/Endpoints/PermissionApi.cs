using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using wx.Application.Security.Permissions;
using wx.Core.Enums;

namespace wx.Api.Endpoints;

public static class PermissionApi
{
    public static RouteGroupBuilder MapPermissionApi(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/permission");
        group.WithTags("Permission");
        group.MapPost("/", AddPermissionAsync);
        group.MapDelete("/{permissionId:int}", DeletePermissionAsync);
        return group;
    }

    public static async Task<Results<NoContent, BadRequest, Conflict>> AddPermissionAsync(
        [FromBody] AddPermissionRqeuset request,
        [FromServices] IMediator mediator)
    {
        var cmd = new CreatePermissionCommand(request.Code, request.Name, PermissionType.Action);
        await mediator.Send(cmd);
        return TypedResults.NoContent();
    }

    public static async Task<Results<NoContent, NotFound, BadRequest>> DeletePermissionAsync(
        [FromRoute] int permissionId, [FromServices] IMediator mediator)
    {
        var cmd = new DeletePermissionCommand(permissionId);
        await mediator.Send(cmd);
        return TypedResults.NoContent();
    }
}

public record AddPermissionRqeuset(string Code, string Name);