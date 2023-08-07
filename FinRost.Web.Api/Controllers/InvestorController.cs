using AutoMapper;
using FinRost.BL.Dto.Web.Investor;
using FinRost.BL.Services;
using FinRost.DAL.Dto;
using FinRost.DAL.Entities.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinRost.Web.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class InvestorController : ControllerBase
    {
        private readonly InvestorService _investorService;
        private readonly ChatUserService _chatUserService;
        private readonly IMapper _mapper;
        public InvestorController(IMapper mapper, InvestorService investorService, ChatUserService chatUserService)
        {
            _investorService = investorService;
            _mapper = mapper;
            _chatUserService = chatUserService;
        }

        /// <summary>
        /// Поиск инвестора по имени/фамилии
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("investors")]
        public async Task<ActionResult<List<InvestorResponse>>> GetInvestors(string name = "")
        {
            var investors = await _investorService.GetInvestorsAsync(name);

            var investorsResponse = _mapper.Map<List<InvestorResponse>>(investors);

            return Ok(investorsResponse);
        }

        /// <summary>
        /// Получение инвестра по Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        [HttpGet("investors/{Id}")]
        public async Task<ActionResult<InvestorResponse>> GetInvestorById(int Id)
        {
            var investor = await _investorService.GetInvestorByIdAsync(Id);
            if (investor is null)
            {
                return BadRequest(new ErrorResponse { Message = "Инвестор не найден!" });
            };

            var response = _mapper.Map<InvestorResponse>(investor);
            return Ok(response);
        }

        /// <summary>
        /// Создание инвестора
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("investors")]
        public async Task<ActionResult<InvestorResponse>> CreateInvestor(InvestorRequest request)
        {
            var newInvestor = await _investorService.CreateInvestorAsync(request);

            return Ok(_mapper.Map<InvestorResponse>(newInvestor));
        }

        /// <summary>
        /// Редактирование инвестора
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("investors")]
        public async Task<ActionResult<InvestorResponse>> EditInvestor(InvestorRequest request)
        {
            var investor = await _investorService.EditInvestorAsync(request);

            var response = _mapper.Map<InvestorResponse>(investor);

            return Ok(response);
        }

        /// <summary>
        /// Удаление инвестора
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("investors")]
        public async Task<ActionResult> DeleteInvestor(int Id)
        {
            await _investorService.DeleteInvestorAsync(Id);

            return Ok(new ApiResponse<string> { Message = "Инвестор удален!" });
        }

        /// <summary>
        /// Получение подписавшихся пользователей
        /// </summary>
        /// <param name="name"> ChatId, FirstName,LastName,UserName</param>
        /// <returns></returns>

        [HttpGet("chatUsers")]
        public async Task<ActionResult<List<ChatUser>>> GetChatUsers(string name = "")
        {
            var chatUsers = await _chatUserService.GetChatUsersAsync(name);

            return Ok(chatUsers);
        }

    }
}
