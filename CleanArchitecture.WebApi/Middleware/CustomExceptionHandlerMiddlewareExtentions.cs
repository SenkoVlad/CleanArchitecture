using Microsoft.AspNetCore.Builder;

namespace CleanArchitecture.WebApi.Middleware
{
    public static class CustomExceptionHandlerMiddlewareExtentions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}
