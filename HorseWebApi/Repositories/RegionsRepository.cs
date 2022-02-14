using HorseWebApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HorseWebApi.Repositories
{
    public class RegionsRepository : GenericRepository<Region>
    {
        public RegionsRepository(ApplicationDBContext context)
            : base(context)
        { }

        // TODO Корректно ли расширять GenericRepository?

        public async Task<IEnumerable<Region>> GetParents()
        {
            var parents = await FindAllAsync(x => x.Parent == null);

            foreach (var t in parents)
                t.Regions = await LoadChilds(t);

            return parents;
        }

        public async Task<Region> GetByIdWithCilds(int id)
        {
            var region = await FindAsync(x => x.Id == id);
            if (region is not null) 
                region.Regions = await LoadChilds(region);
            return region;
        }

        private async Task<List<Region>> LoadChilds(Region reg)
        {
            // TODO Реализовать корректную функцию

            var result = await FindAllAsync(x => x.Parent == reg);

            foreach (var t in result)
            {
                t.Regions = await LoadChilds(t);
            }

            return result.ToList();
        }
    }
}
