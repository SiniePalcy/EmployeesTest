using Employees.Infrastructure.Exceptions;
using Employees.Shared.Model;
using Employees.Shared.Responses;

namespace Employees.API.Middleware;

public class ExceptionMiddleware : IMiddleware
{
    private readonly IWebHostEnvironment _env;
    public ExceptionMiddleware(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex) when (ex is ObjectNotFoundException)
        {
            await WriteExceptionData(context, StatusCodes.Status400BadRequest, ex);
        }
        catch (Exception ex)
        {
            await WriteExceptionData(context, StatusCodes.Status500InternalServerError, ex);
        }
    }

    private async Task WriteExceptionData(HttpContext context, int statusCode, Exception ex)
    {
        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        var errorResponse = ConstructExceptionResponse(ex, _env.IsDevelopment() || _env.IsStaging());
        await context.Response.WriteAsJsonAsync(errorResponse);
    }

    private static ErrorResponse ConstructExceptionResponse(Exception ex, bool useStackTrace = false)
    {
        while (ex.InnerException != null)
        {
            ex = ex.InnerException;
        }

        ErrorResponse errorResponse = new()
        {
            Message = ex.Message
        };

        if (ex.Data.Keys.Count > 0)
        {
            errorResponse.Errors = new();
        }

        foreach (object key in ex.Data.Keys)
        {
            errorResponse.Errors!.Add(new ErrorDto
            {
                Key = key.ToString()!,
                Description = ex.Data[key]!.ToString()!
            });
        }

        if (useStackTrace)
        {
            errorResponse.StackTrace = ex.StackTrace!;
        }

        return errorResponse;
    }
}
