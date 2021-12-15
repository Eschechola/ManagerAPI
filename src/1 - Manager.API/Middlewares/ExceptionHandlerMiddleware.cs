using Manager.API.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace manager.API.Middlewares
{
    public static class ExceptionHandlerMiddleware
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                        await context.Response.WriteAsJsonAsync(Responses.InternalServerErrorMessage());
                });
            });
        }
    }

}
