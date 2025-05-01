using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using DsaApi.Application.Exceptions; // Reference your custom exceptions

namespace DsaApi.WebApi.Middleware
{
    // Example of a simple global exception handler
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred.");

            // Default to 500 Internal Server Error
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string title = "An unexpected error occurred.";
            string detail = exception.Message; // Don't expose stack trace in production

            // Customize based on exception type
            switch (exception)
            {
                case EmptyStackOperationException esoe:
                    statusCode = HttpStatusCode.BadRequest; // 400
                    title = "Invalid Operation";
                    detail = esoe.Message;
                    break;
                case ArgumentNullException ane:
                    statusCode = HttpStatusCode.BadRequest; // 400
                    title = "Invalid Argument";
                    detail = ane.Message;
                    break;
                case ArgumentException ae:
                    statusCode = HttpStatusCode.BadRequest; // 400
                    title = "Invalid Argument";
                    detail = ae.Message;
                    break;
                    // Add more custom exception types here
                    // case NotFoundException nfe:
                    //     statusCode = HttpStatusCode.NotFound; // 404
                    //     title = "Resource Not Found";
                    //     detail = nfe.Message;
                    //     break;
            }

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)statusCode;

            var problemDetails = new
            {
                Status = (int)statusCode,
                Title = title,
                Detail = detail,
                // TraceId = System.Diagnostics.Activity.Current?.Id ?? context.TraceIdentifier // Optional: for correlation
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
    }
}

// Remember to register and use this in Program.cs if you implement it:
// builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();
// app.UseMiddleware<GlobalExceptionHandlerMiddleware>();