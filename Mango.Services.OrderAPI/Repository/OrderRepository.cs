using Mango.Services.OrderAPI.DbContexts;
using Mango.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderRepository(IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            _db = new ApplicationDbContext(optionsBuilder.Options);
        }

        public async Task<bool> AddOrderAsync(OrderHeader orderHeader)
        {
            //await using var _db = new ApplicationDbContext(_dbContext);
            _db.OrderHeaders.Add(orderHeader);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task UpdateOrderPaymentStatusAsync(int orderHeaderId, bool paid)
        {
            //await using var _db = new ApplicationDbContext(_dbContext);
            var orderHeader = await _db.OrderHeaders.FirstOrDefaultAsync(o => o.OrderHeaderId == orderHeaderId);
            if (orderHeader != null)
            {
                orderHeader.PaymentStatus = paid;
                //context.OrderHeaders.Update(orderHeader);
                await _db.SaveChangesAsync();
            }
        }
    }
}
