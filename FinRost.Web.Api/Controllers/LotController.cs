using AutoMapper;
using FinRost.BL.Dto.Web.Lots;
using FinRost.BL.Services;
using FinRost.DAL.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;
using Telegram.Bot.Types;

namespace FinRost.Web.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class LotController : ControllerBase
    {
        private readonly LotService _lotService;
        private readonly IMapper _mapper;
        public LotController(LotService lotService, IMapper mapper)
        {
            _lotService = lotService;
            _mapper = mapper;
        }

        /// <summary>
        /// Поиск лота по orderId
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("lots/{orderId}")]
        public async Task<ActionResult<LotResponse>> GetLotByOrderId(int orderId)
        {
            var lot = await _lotService.GetLotByOrderIdAsync(orderId);
            if (lot is null)
                return NoContent();

            var response = _mapper.Map<LotResponse>(lot);
            return Ok(response);
        }

        /// <summary>
        /// Создание лота
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("lots")]
        public async Task<ActionResult<LotResponse>> CreateLot(LotRequest request)
        {
            var lot = await _lotService.CreateLotAsync(request);
            return Ok(lot);
        }

        /// <summary>
        /// Редактирование лота
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("lots")]
        public async Task<ActionResult<LotResponse>> EditLot(LotRequest request)
        {
            var lot = await _lotService.EditLotAsync(request);
            return Ok(lot);
        }

        /// <summary>
        /// Удаление Лота
        /// </summary>
        /// <param name="Id"> Лот Id</param>
        /// <returns></returns>
        [HttpDelete("lots")]
        public async Task<ActionResult> DeleteLot(int Id)
        {
            await _lotService.DeleteLotAsync(Id);
            await _lotService.DeleteFeedBacksAsync(Id);
            return Ok();
        }

        /// <summary>
        /// Получение ссылок на файлы лота
        /// </summary>
        /// <param name="Id"> Лот Id</param>
        /// <returns></returns>
        [HttpGet("lots/{Id}/files/links")]
        public async Task<ActionResult<List<FileLink>>> GetLotFileLinks(int Id)
        {
            var fileNames = await _lotService.GetLotFileNamesAsync(Id);

            var response = new List<FileLink>();

            foreach (var file in fileNames)
            {
                response.Add(new FileLink
                {
                    FileName = file,
                    Link = Url.Action("GetLotFile", new { Id = Id, name = file})
                });
            }


            return Ok(response);
        }

        /// <summary>
        /// Добавление файла в лот
        /// </summary>
        /// <param name="Id">Id Лота</param>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost("lots/{Id}/files"), DisableRequestSizeLimit]
        public async Task<ActionResult> AddFiles(int Id, IFormFileCollection files)
        {
            if (files is null || files.Count == 0)
                return BadRequest(new ErrorResponse
                {
                    Message = "Нет файлов в запросе!"
                });

            var listFiles = new List<FileDto>();

            listFiles = files.Select(file => new FileDto
            {
                FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName),
                FileStream = file.OpenReadStream()
            }).ToList();

            await _lotService.AddFilesAsync(Id,listFiles);

            return Ok();
        }

        /// <summary>
        /// Получение файла по ссылке
        /// </summary>
        /// <param name="Id"> Лот Id</param>
        /// <param name="name"> Имя файла</param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("/files/lotImages/{Id}/{name}")]
        [HttpGet]
        public async Task<ActionResult> GetLotFile(int Id, string name)
        {
            var path = await _lotService.GetPathToFile(Id, name);

            var file = new FileInfo(path);
            if (!file.Exists)
                return BadRequest(new ErrorResponse
                {
                    Message = "Ошибка получения файла! Файл не существует!"
                });

            var fileProvider = new FileExtensionContentTypeProvider();
            if (!fileProvider.TryGetContentType(name, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(fileStream, contentType);
        }

        /// <summary>
        /// Удаление файла
        /// </summary>
        /// <param name="Id"> Лот Id</param>
        /// <param name="name"> Имя файла</param>
        /// <returns></returns>
        [Route("/files/lotImages/{Id}/{name}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteLotFile(int Id, string name)
        {
            var path = await _lotService.GetPathToFile(Id, name);

            var file = new FileInfo(path);
            if (!file.Exists)
                return BadRequest(new ErrorResponse
                {
                    Message = "Ошибка получения файла! Файл не существует!"
                });

            file.Delete();
            return Ok(new DAL.Dto.ApiResponse<string>() { Message = "Файл удален!"});
        }

        /// <summary>
        /// Получение отликов на лот
        /// </summary>
        /// <param name="Id">Лот Id</param>
        /// <returns></returns>
        [HttpGet("lots/{Id}/feedbacks")]
        public async Task<ActionResult<List<LotFeedbackDto>>> GetLotFeedbacks(int Id)
        {
            var feedbacks = await _lotService.GetLotFeedbacksAsync(Id);
            return Ok(feedbacks);
        }

        /// <summary>
        /// Назначение инвестора на лот
        /// </summary>
        /// <param name="Id">Лот Id</param>
        /// <param name="investorId"> Инвестор Id</param>
        /// <returns></returns>
        [HttpPost("lots/{Id}/investor/{investorId}")]
        public async Task<ActionResult> SetLotInvestor(int Id, int investorId)
        {
            await _lotService.SetLotInvestorAsync(Id, investorId);
            return Ok();
        }

    }
}
