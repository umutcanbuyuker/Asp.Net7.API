using Asp.Net7.API.Middlewares;

namespace Asp.Net7.API.Extensions
{
    public static class ExceptionHandleMiddlewareExtension
    {
        public static IApplicationBuilder UseExceptionHandleMiddle(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandleMiddleware>();
        }
    }
}
