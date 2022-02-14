using AutoMapper;
using HorseWebApi.Entities;
using HorseWebApi.Infrastructure;
using HorseWebApi.Repositories;
using HorseWebApi.Requests;
using HorseWebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HorseWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrdersRepository ordersrepository;
        private readonly RegionsRepository regionsRepository;
        private readonly IMapper mapper;

        public OrdersController(
            IGenericRepository<Order> ordersrepository,
            IGenericRepository<Region> regionsRepository,
            IMapper mapper)
        {
            this.regionsRepository = regionsRepository as RegionsRepository ??
                 throw new ArgumentNullException(nameof(regionsRepository));
            this.ordersrepository = ordersrepository as OrdersRepository ??
                 throw new ArgumentNullException(nameof(ordersrepository));
            this.mapper = mapper ??
                 throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<OrderVM>>> Get(string itemName, int? regionId, int? page, int? pageSize)
        {
            IQueryable<Order> query = this.ordersrepository.Query()
                                                           .Include(x => x.User)
                                                           .Include(x => x.Items)
                                                           .ThenInclude(x => x.Item);

            if (itemName is not null)
                query = query = query.Where(x => x.Items.Any(y => y.Item.Name == itemName));

            if (regionId is not null)
            {
                query = query = query.Where(x => x.Region.Id == regionId);

                var childRegions = await regionsRepository.GetByIdWithCilds(regionId.Value);
            }

            if (page is not null && pageSize is not null)
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            var result = query.ToList();

            return result is null
            ? NotFound()
            : Ok(result.Select(x => HorseMapper.Map(x)));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Item>> Get(int id)
        {
            var result = await this.ordersrepository.FindAsync(x => x.Id == id);

            return result is null
            ? NotFound()
            : Ok(HorseMapper.Map(result));
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult> Post([FromBody] OrderRequest item)
        {
            var order = mapper.Map<Order>(item);
            order.Date = DateTime.Now.AddDays(30);
            await this.ordersrepository.AddAsync(order);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Put([FromBody] Order item)
        {
            var result = await this.ordersrepository.FindAsync(x => x.Id == item.Id);

            if (result is not null)
            {
                this.ordersrepository.Update(item);
                return Ok();
            }
            else
                return NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var item = await this.ordersrepository.FindAsync(x => x.Id == id);

            if (item is not null)
            {
                this.ordersrepository.Delete(item);
                return Ok();
            }
            else
                return NotFound();
        }
    }
}
