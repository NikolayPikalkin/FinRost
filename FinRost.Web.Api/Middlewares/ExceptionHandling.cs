using FinRost.BL.Extensions;
using FinRost.BL.Services;
using FinRost.DAL.Dto;
using FinRost.Web.Api.Extensions;
using Telegram.Bot;

namespace FinRost.Web.Api.Middlewares
{
    public class ExceptionHandling
    {
        static readonly ITelegramBotClient bot = new TelegramBotClient("X");
        static readonly long chatId = X;

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandling> _logger;

        public ExceptionHandling(
            RequestDelegate next,
            ILogger<ExceptionHandling> logger
            )
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext, LogService logService)
        {
            try
            {
                await _next(httpContext);
            }
            catch (CustomException ex)
            {
                var exMessage = await HandlingException(ex, httpContext, logService);

                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsJsonAsync(new ErrorResponse
                {
                    Message = ex.Message,
                    Details = exMessage
                });
            }
            catch (Exception ex)
            {
                var exMessage = await HandlingException(ex, httpContext, logService);

                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsJsonAsync(new ErrorResponse
                {
                    Details = exMessage,
                    Message = "Ошибка сервера! Повторите попытку позже!"
                });
            }
        }

        //TODO: Доделать обработку ошибок
        private async Task<string> HandlingException(Exception ex, HttpContext httpContext, LogService logService)
        {
            var logMessage = "Error = " + ex.Message;

            if (ex.InnerException != null)
                logMessage += "InnerEx: " + ex.InnerException;

            var user = httpContext.User;

            if (user is null)
                logMessage += $"\nIpAddress = {httpContext.Connection.RemoteIpAddress}";
            else
                logMessage += $"\nUserId = {httpContext.GetCurrentUserId()} " +
                              $"\nUserName = {httpContext.GetUserFullName()}";

            logMessage += $"\nMethod = {httpContext.Request.Method} {httpContext.Request.Path} ";

            _logger.LogError(logMessage);

            await logService.AddError(logMessage);

            await bot.SendTextMessageAsync(chatId,logMessage);

            return logMessage;
        }

    }
}
