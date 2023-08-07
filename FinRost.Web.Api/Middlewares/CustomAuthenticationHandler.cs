using FinRost.BL.Services.Auth;
using FinRost.DAL.Dto;
using FinRost.DAL.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace FinRost.Web.Api.Middlewares
{
    public class CustomAuthenticationSchemeOptions : AuthenticationSchemeOptions { }

    public class CustomAuthenticationHandler : AuthenticationHandler<CustomAuthenticationSchemeOptions>
    {
        private class TokenModel
        {
            public int UserId { get; set; }
            public string Name { get; set; }
            public string Login { get; set; }
        }

        private readonly UserService _userService;
        private readonly AesCriptoService _cripto;
        public CustomAuthenticationHandler(
            IOptionsMonitor<CustomAuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            AesCriptoService cripto,
            UserService userService)
            : base(options, logger, encoder, clock)
        {
            _cripto = cripto;
            _userService = userService;
        }
        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var token = Request.Headers["token"].ToString() ?? string.Empty;

            if (string.IsNullOrEmpty(token))
                return AuthenticateResult.Fail("Ошибка авторизации!");

            var validToken = Convert.TryFromBase64String(token, new Span<byte>(new byte[token.Length]), out int bytes);
            if (!validToken)
                return AuthenticateResult.Fail("Invalid Authorization Key! Error convert!");

            var userData = _cripto.DecryptStringFromBytes_Aes(Convert.FromBase64String(token));
            ParseToken(/*------PRIVATE--------*/);

            var user = await _userService.FindUserByIdAndLogin(/*------PRIVATE--------*/);
            if (user is null || !user.Enabled)
                return AuthenticateResult.Fail("Пользователь не найден!");

            if (user.Blocked)
                return AuthenticateResult.Fail("Учетная запись заблокирована!");

            var tokenModel = new TokenModel()
            {
                Login = user.Login,
                Name = user.FullName,
                UserId = user.Id
            };

            var claims = new List<Claim>
            {
                    new Claim("Id", tokenModel.UserId.ToString()),
                    new Claim("Login", tokenModel.Login),
                    new Claim(ClaimTypes.Name, tokenModel.Name)
            };

            var claimsIdentity = new ClaimsIdentity(claims,
                        nameof(CustomAuthenticationHandler));

            var ticket = new AuthenticationTicket(
                new ClaimsPrincipal(claimsIdentity), Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            await base.HandleChallengeAsync(properties);
            if (Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                await Context.Response.WriteAsJsonAsync(
                    new ErrorResponse
                    {
                        Message = (await Context.AuthenticateAsync()).Failure?.Message
                    });
            }
        }

        private static void ParseToken(/*------PRIVATE--------*/)
        {
            /*------PRIVATE--------*/
        }

    }

}
