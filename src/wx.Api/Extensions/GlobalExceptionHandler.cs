using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace wx.Api.Extensions;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {

        var problemDetail = exception switch
        {
            KeyNotFoundException ex => HandleKeyNotFoundException(ex, httpContext),
            _ => HandleUnhandleException(exception, httpContext)

        };

        httpContext.Response.StatusCode = problemDetail.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetail);
        
        return true;
    }

    private ProblemDetails HandleKeyNotFoundException(KeyNotFoundException ex,HttpContext httpContext)
    {
        //RFC 7807
        //urn - urn:errors:not-found
        //dynamic - {httpContext.Request.Scheme}://{httpContext.Request.Host}/errors/not-found
        //doc - https://tools.ietf.org/html/rfc7231#section-6.5.4
        return new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Not Found",
            Detail = ex.Message,
            Instance = httpContext.Request.Path,
            Type = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/errors/not-found"
        };
    }

    private ProblemDetails HandleKeyValidationException(KeyNotFoundException ex, HttpContext httpContext)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Error",
            Detail = ex.Message,
            Instance = httpContext.Request.Path,
            Type = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/errors/validation-error"
        };
    }

    private ProblemDetails HandleUnhandleException(Exception ex, HttpContext httpContext)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server Error",
            Detail = ex.Message,
            Instance = httpContext.Request.Path,
            Type = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/errors/internal-server-error"
        };
    }
}
