using FinRost.BL.Dto.Web.Log;
using FinRost.BL.Services;
using FinRost.DAL.Dto;
using FinRost.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinRost.Web.Api.Controllers
{
    [Route("api/logs")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class LogController : ControllerBase
    {
        private readonly LogService _logService;
        public LogController(LogService logService)
        {
            _logService = logService;
        }
        /// <summary>
        /// Ошибки бэка
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("back")]
        public async Task<ActionResult<LogResponse>> GetBackLogs(int page = 1)
        {
            return await _logService.GetLogsAsync(0,page);
        }

        /// <summary>
        /// Ошибки фронта
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("front")]
        public async Task<ActionResult<LogResponse>> GetFrontLogs(int page = 1)
        {
            return await _logService.GetLogsAsync(1, page);
        }

        /// <summary>
        /// Добавление ошибка фронта
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("front")]
        public async Task<ActionResult> AddFrontLog(LogRequest request)
        {
            var log = await _logService.AddFrontLogAsync(request);
            return Ok(new ApiResponse<ServiceLog> {Data = log, Message = "Ошибка добавлена!" });
        }


    }
}
