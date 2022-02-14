using HorseWebApi.Repositories;
using HorseWebApi.Entities;

namespace HorseWebApi.Infrastructure
{
    public class ItemsRepository : GenericRepository<Item>
    {
        public ItemsRepository(ApplicationDBContext context)
            : base(context)
        { }
    }
}
