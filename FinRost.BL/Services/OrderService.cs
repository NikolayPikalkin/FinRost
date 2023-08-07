using FinRost.BL.Dto.Web.Orders;
using FinRost.DAL;
using FinRost.DAL.Entities.Archi;
using Microsoft.EntityFrameworkCore;

namespace FinRost.BL.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _db;
        public OrderService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<OrderResponse>> GetOrdersAsync(string numberOrName)
        {
            var orders = await _db.Orders.Where(it => it.ORDERSTATUS == 2 && it.ENABLED == 1 && (it.NUMBER.Contains(numberOrName) || it.FULLNAME.Contains(numberOrName)) )
                                         .OrderByDescending(it => it._CreationDateTime)
                                         .Take(30)
                                         .Join(_db.RelisePlaces, or => or.RELISEPLACEID, pl => pl.Id, (or, pl) =>
                                         new OrderResponse
                                         {
                                             ClientId = or.CLIENTID,
                                             ClientName = or.FULLNAME,
                                             CreationDate = DateTime.FromOADate(or._CreationDateTime),
                                             Id = or.ID,
                                             Number = or.NUMBER,
                                             RelisePlace = pl.Name
                                         }).ToListAsync();

            foreach (var or in orders)
            {
                var lot = await _db.Lots.FirstOrDefaultAsync(it => it.OrderId == or.Id && it.Enabled);
                if (lot != null)
                {
                    or.IsLot = true;
                    or.CountFeedbacks = await _db.LotFeedbacks.CountAsync(it => it.LotId == lot.Id && it.Enabled);
                }   
            }

            return orders.OrderByDescending(it => it.CountFeedbacks).ToList();
        }

        public async Task<Order?> GetOrderByIdAsync(int Id)
        {
            return await _db.Orders.FindAsync(Id);
        }

    }
}
