namespace TaskManagement.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType =
                "application/json";

            context.Response.StatusCode = 500;

            await context.Response
                .WriteAsJsonAsync(new
                {
                    success = false,
                    message = ex.Message
                });
        }
    }
}