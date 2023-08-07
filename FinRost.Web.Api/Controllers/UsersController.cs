using AutoMapper;
using FinRost.BL.Dto.Web.Users;
using FinRost.DAL.Dto;
using FinRost.DAL.Entities.Archi;
using FinRost.DAL.Services;
using FinRost.Web.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinRost.Web.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public UsersController(UserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение информации по профилю
        /// </summary>
        /// <returns></returns>
        [HttpGet("profile")]
        public async Task<ActionResult<ProfileResponse>> GetProfileInfo()
        {
            var user = await _userService.GetUserByIdAsync(HttpContext.GetCurrentUserId());

            var response = _mapper.Map<ProfileResponse>(user);
            return Ok(response);
        }

        /// <summary>
        /// Получение всех пользователей
        /// </summary>
        /// <returns></returns>
        [HttpGet("users")]
        public async Task<ActionResult<List<UserResponse>>> GetUsers()
        {
            var listUsers = await _userService.GetUsersAsync();
            var userResponses = _mapper.Map<List<UserResponse>>(listUsers);

            return Ok(userResponses);
        }

        /// <summary>
        /// Получение пользователя по Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("users/{Id}")]
        public async Task<ActionResult<UserDetailResponse>> GetUserById(int Id)
        {
            var user = await _userService.GetUserByIdAsync(Id);
            if (user is null)
                return BadRequest(new ErrorResponse
                {
                    Message = "Пользователь с таким Id не найден!",
                });

            return Ok(_mapper.Map<UserDetailResponse>(user));
        }

        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("users")]
        public async Task<ActionResult<UserDetailResponse>> CreateUser(UserRequest request)
        {
            var newUser = await _userService.CreateUser(request);
            var userResponse = _mapper.Map<UserDetailResponse>(newUser);
            return Ok(userResponse);

        }

        /// <summary>
        /// Редактирование пользователя
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("users")]
        public async Task<ActionResult<UserDetailResponse>> EditUser(UserRequest request)
        {
            var newUser = await _userService.EditUser(request);
            var userResponse = _mapper.Map<UserDetailResponse>(newUser);
            return Ok(userResponse);
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("users/{Id}")]
        public async Task<ActionResult> DeleteUser(int Id)
        {
            await _userService.DeleteUser(Id);
            return Ok();
        }

        /// <summary>
        /// Получение пользователей из Арчи
        /// </summary>
        /// <returns></returns>
        [HttpGet("users/archi")]
        public async Task<ActionResult<List<ArchiUser>>> GetArchiUsers()
        {
            return Ok(await _userService.GetArchiUsersAsync());
        }

    }
}
