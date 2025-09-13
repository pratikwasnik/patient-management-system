#region Namespaces
using System.Net;
using System.Text.Json;
using Microsoft.Data.SqlClient; // for SqlException
using System.ComponentModel.DataAnnotations; // for ValidationException
#endregion

namespace PatientManagement.API.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        #region Fields & Constructor

        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        #endregion

        #region Middleware Pipeline

        public async Task Invoke(HttpContext context)
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

        #endregion

        #region Private Helpers

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            context.Response.Clear(); // Clear any previous response
            context.Response.ContentType = "application/json";

            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = "An unexpected error occurred. Please try again later.";

            // Handle duplicate email (SQL unique constraint violation)
            if (ex is SqlException sqlEx && (sqlEx.Number == 2627 || sqlEx.Number == 2601))
            {
                statusCode = (int)HttpStatusCode.Conflict; // 409
                message = "Email cannot be duplicate, user with same Email already exists!";
            }
            // Handle wrong email format (DataAnnotations / FluentValidation)
            else if (ex is ValidationException)
            {
                statusCode = (int)HttpStatusCode.BadRequest; // 400
                message = "Invalid email format. Please provide a valid email address.";
            }
            // Handle not found (e.g., thrown from repository/service)
            else if (ex is KeyNotFoundException)
            {
                statusCode = (int)HttpStatusCode.NotFound; // 404
                message = "The requested entity was not found.";
            }

            context.Response.StatusCode = statusCode;

            var payload = new
            {
                status = statusCode,
                error = message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }

        #endregion
    }
}
