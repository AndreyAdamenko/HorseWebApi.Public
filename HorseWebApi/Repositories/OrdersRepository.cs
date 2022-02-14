using HorseWebApi.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HorseWebApi.Repositories
{
    public class OrdersRepository : GenericRepository<Order>
    {
        public OrdersRepository(ApplicationDBContext context) : base(context)
        { }

        public async Task<IEnumerable<Order>> GetRegion(int id)
        {
            SqlParameter param = new("@id", id);
            return await context.Orders.FromSqlRaw("GetOrdersByRegion @id", param).ToListAsync();
        }

        public override Task<Order> AddAsync(Order entity)
        {
            entity.Items.ForEach(async x =>
            {
                this.context.Entry(x).State = EntityState.Added;
                await this.context.Set<OrderItem>().AddAsync(x);
            });

            return base.AddAsync(entity);
        }
    }
}
