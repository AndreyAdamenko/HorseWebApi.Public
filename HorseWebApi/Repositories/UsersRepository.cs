using HorseWebApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HorseWebApi.Repositories
{
    public class UsersRepository : GenericRepository<User>
    {
        public UsersRepository(ApplicationDBContext context) : base(context)
        { }
    }
}
