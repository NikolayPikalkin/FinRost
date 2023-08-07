using AutoMapper;
using FinRost.BL.Dto.Web.Orders;
using FinRost.BL.Services;
using FinRost.DAL.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinRost.Web.Api.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private IMapper _mapper;
        public OrderController(OrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderResponse>>> GetOrders(string numberOrName = "")
        {
            return await _orderService.GetOrdersAsync(numberOrName);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<OrderDetailResponse>> GetOrder(int Id)
        {
            var order = await _orderService.GetOrderByIdAsync(Id);
            if (order is null)
                return BadRequest(new ErrorResponse
                {
                    Message = "Заявка не найдена!"
                });

            var response = new OrderDetailResponse
            {
                Id = order.ID,
                ClientId = order.CLIENTID,
                CloseDateTime = order.CloseDateTime,
                CreationDateTime = order.CreationDateTime,
                Daysquant = order.DAYSQUANT ?? 0,
                EnterRelisePlaceId = order.ENTERRELISEPLACEID,
                FullName = order.FULLNAME,
                LoanCostall = order.LOANCOSTALL,
                LoanRestCostall = order.LOANRESTCOSTALL,
                MainPercent = order.MAINPERCENT ?? 0,
                Number = order.NUMBER,
                OrderStatus = order.ORDERSTATUS,
                PeniCostall = order.PENICOSTALL,
                PercentCostall = order.PERCENTCOSTALL,
                PutDateTime = order.PutDateTime,
                RelisePlaceId = order.RELISEPLACEID,
                ReturnDateTime = order.ReturnDateTime,
                UserId = order.USERID
            };

            return Ok(response);
        }

    }
}
