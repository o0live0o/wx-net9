using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using wx.Application.Security.Users;

namespace wx.Api.Endpoints;

public static class UserApi
{
    public static RouteGroupBuilder MapUsersApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/users");
        api.WithTags("User");
        api.MapPost("/", CreateUserAsync);
        api.MapGet("/", GetUsersAsync);
        api.MapDelete("/", DeleteUserAsync);
        api.MapPost("/{userId:int}/roles", AssignRolesAsync);
        return api;
    }

    public static async Task<Results<Ok, BadRequest, Conflict>> CreateUserAsync([FromBody] CreateUserCommand request,
        [FromServices] IMediator mediator)
    {
        await mediator.Send(request);
        return TypedResults.Ok();
    }

    public static async Task<Ok<IEnumerable<UserDto>>> GetUsersAsync([FromServices] IUserQuery userQuery)
    {
        var users = await userQuery.GetAllUsersAsync();
        return TypedResults.Ok(users);
    }

    public static async Task<Results<NoContent, NotFound>> DeleteUserAsync([FromBody] DeleteUserCommand request,
        [FromServices] IMediator mediator)
    {
        await mediator.Send(request);
        return TypedResults.NoContent();
    }

    public static async Task<Results<NoContent, BadRequest, NotFound>> AssignRolesAsync([FromRoute] int userId,
        [FromBody] AssignRolesRequest request,
        [FromServices] IMediator mediator)
    {
        var cmd = new AssignRolesCommand(userId, request.RoleIds);
        await mediator.Send(cmd);
        return TypedResults.NoContent();
    }
}

public record AssignRolesRequest(int[] RoleIds);