using AutoMapper;
using FinRost.BL.Dto.Web.Notifications;
using FinRost.BL.Services;
using FinRost.Web.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinRost.Web.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationService _notificationService;
        private readonly IMapper _mapper;

        public NotificationController(NotificationService notificationService, IMapper mapper)
        {
            _notificationService = notificationService;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение групп уведомлений 
        /// </summary>
        /// <returns></returns>
        [HttpGet("notifyGroups")]
        public async Task<ActionResult<List<NotifyGroupResponse>>> GetNotifyGroups()
        {
            var notifyGroups = await _notificationService.GetNotifyGroupsAsync();

            var response = new List<NotifyGroupResponse>();

            foreach(var it in notifyGroups)
            {
                response.Add(new NotifyGroupResponse
                {
                    GroupName = it.Name,
                    Id = it.Id,
                    KeyWord = it.KeyWord,
                });
            }

            return Ok(response);
        }

        /// <summary>
        /// Получение информации о группе уведомлений
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("notifyGroups/{Id}")]
        public async Task<ActionResult<NotifyGroupResponse>> GetNotifyGroup(int Id)
        {
            return Ok(await _notificationService.GetNotifyGroupByIdAsync(Id));
        }


        /// <summary>
        /// Создание новой группы уведомлений
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("notifyGroups")]
        public async Task<ActionResult<NotifyGroupResponse>> CreateNotifyGroup(NotifyGroupRequest request)
        {
            var newGroup = await _notificationService.CreateNotifyGroupAsync(request);
            return Ok(await _notificationService.GetNotifyGroupByIdAsync(newGroup.Id));
        }

        /// <summary>
        /// Редактирование группы уведомлений
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("notifyGroups")]
        public async Task<ActionResult<NotifyGroupResponse>> EditNotifyGroup(NotifyGroupRequest request)
        {
            await _notificationService.EditNotifyGroupAsync(request);
            return Ok(await _notificationService.GetNotifyGroupByIdAsync(request.Id));
        }

        /// <summary>
        /// Удаление группы
        /// </summary>
        /// <param name="Id">Id группы</param>
        /// <returns></returns>
        [HttpDelete("notifyGroups")]
        public async Task<ActionResult> DeleteNotifyGroup(int Id)
        {
            await _notificationService.DeleteNotifyGroupAsync(Id);
            return NoContent();
        }

        /// <summary>
        /// Получение уведомлений профиля
        /// </summary>
        /// <returns></returns>
        [HttpGet("profileNotices")]
        public async Task<ActionResult<List<NotificationDto>>> GetProfileNotices()
        {
            var notifies = await _notificationService.GetProfileNoticesAsync(HttpContext.GetCurrentUserId());

            var response = _mapper.Map<List<NotificationDto>>(notifies);

            return Ok(response);

        }
        /// <summary>
        /// Прочитать все уведомления
        /// </summary>
        /// <returns></returns>
        [HttpPost("profileNotices/checkAll")]
        public async Task<ActionResult> CheckAllProfileNotices()
        {
            await _notificationService.CheckAllProfileNoticesAsync(HttpContext.GetCurrentUserId());

            return Ok();
        }
    
    
    
    
    
    
    }
}
