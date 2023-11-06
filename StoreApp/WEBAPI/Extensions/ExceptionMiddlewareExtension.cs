using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json.Linq;
using Project.Entity.Enums;
using Project.Entity.ErrorModel;
using Project.Entity.Exceptions;
using Project.Services.Abstract;
using System.Linq.Expressions;
using System.Net;

namespace WEBAPI.Extensions
{
    public static class ExceptionMiddlewareExtension
    {
        public static void ConfigureExceptionHandler(this WebApplication app, ILoggerService logger)
        {
            app.UseExceptionHandler(err =>
            {
                err.Run(async context =>
                {
                    //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // default assigned
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>(); // can be null
                    if(contextFeature is not null)
                    {
                        //switch (contextFeature.Error)
                        //{
                        //    case NotFoundException:
                        //        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        //        break;
                        //    default:
                        //        context.Response.StatusCode = StatusCodes.Status500InternalServerError; 
                        //        break;
                        //}
                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound, // case 1
                            NullObjectException => StatusCodes.Status405MethodNotAllowed, // case 2 
                            _ => StatusCodes.Status500InternalServerError, // _ default
                        };
                         
                        logger.Log($"Something went wrong : {contextFeature.Error.Message}", LogType.Error);
                        await context.Response.WriteAsync(new ErrorDetails
                        {
                            Message = contextFeature.Error.Message,
                            StatusCode = context.Response.StatusCode,
                        }.ToString());
                    }
                });
            });
        }
    }
}
