using FinRost.BL.Services.Auth;
using FinRost.DAL.Dto;
using FinRost.DAL.Dto.Login;
using FinRost.Web.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinRost.Web.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Produces("application/json")]
    public class LoginController : ControllerBase
    {

        private readonly LoginService _login;
        public LoginController(LoginService login)
        {
            _login = login;
        }

        /// <summary>
        /// Получение токена
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var authResult = await _login.Login(request, Request.GetIpAddress().ToString());

            if (!string.IsNullOrEmpty(authResult.Token) && string.IsNullOrEmpty(authResult.Message))
                return Ok(authResult);

            return BadRequest(new ErrorResponse { Message = authResult.Message });
        }
    }
}
