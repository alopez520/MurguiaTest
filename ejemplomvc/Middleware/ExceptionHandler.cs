using ejemplomvc.DTOs;
using ejemplomvc.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace ejemplomvc.Middleware
{
    public static class ExceptionHandler
    {
        public static void UseApiExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        //logger.Error($"Something went wrong: {contextFeature.Error}");
                        var error = contextFeature.Error;
                        if (error is not null && error is ServiceException)
                        {

                            await context.Response.WriteAsync(
                                JsonSerializer.Serialize(
                                    ApiResponseDto<object>.Error(
                                        error.Message,
                                        errores: [((ServiceException)error).Message]
                                    )
                                )
                            );
                        }
                        else if (error is not null && error is RepositoryException)
                        {

                            await context.Response.WriteAsync(
                                JsonSerializer.Serialize(
                                    ApiResponseDto<object>.Error(
                                        error.Message,
                                        errores: [((RepositoryException)error).Message]
                                    )
                                )
                            );
                        }
                        else
                        {
                            await context.Response.WriteAsync(
                                JsonSerializer.Serialize(
                                     ApiResponseDto<object>.Error(
                                        "ocurrio un error inesperado",
                                        errores: [contextFeature.Error.Message]
                                    )
                                )
                            );
                        }
                    }
                });
            });
        }
    }
}
