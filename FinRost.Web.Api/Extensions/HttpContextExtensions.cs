using System.Security.Claims;

namespace FinRost.Web.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static int GetCurrentUserId(this HttpContext context)
        {
            return int.Parse(context.User.Claims.FirstOrDefault(it => it.Type == "Id")?.Value ?? "0");
        }

        public static string GetUserFullName(this HttpContext context)
        {
            return context.User.Claims.FirstOrDefault(it => it.Type == ClaimTypes.Name)?.Value ?? string.Empty;
        }

        public static string GetIpAddress(this HttpRequest request)
        {
            return request.HttpContext.Connection.RemoteIpAddress.ToString();
        }

    }
}
