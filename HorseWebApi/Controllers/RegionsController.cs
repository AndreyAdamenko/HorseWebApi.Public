using AutoMapper;
using HorseWebApi.Entities;
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
    public class RegionsController : ControllerBase
    {
        private readonly RegionsRepository repository;
        private readonly IMapper mapper;

        public RegionsController(
            IMapper mapper,
            IGenericRepository<Region> repository)
        {
            this.mapper = mapper ??
                 throw new ArgumentNullException(nameof(mapper));
            this.repository = repository as RegionsRepository ??
                 throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<RegionVM>>> Get()
        {
            var parents = await this.repository.GetParents();

            return parents is null
            ? NotFound()
            : Ok(this.mapper.Map<IEnumerable<RegionVM>>(parents));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<RegionVM>> Get(int id)
        {
            var region = await this.repository.GetByIdWithCilds(id);

            return region is null
            ? NotFound()
            : Ok(this.mapper.Map<RegionVM>(region));
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Post([FromBody] RegionRequest item)
        {
            // TODO Как должна выглядеть модель для добавления региона
            await this.repository.AddAsync(this.mapper.Map<Region>(item));

            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Put([FromBody] RegionVM item)
        {
            var result = await this.repository.FindAsync(x => x.Id == item.Id);

            if (result is not null)
            {
                this.repository.Update(this.mapper.Map<Region>(item));
                return Ok();
            }
            else
                return NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var item = await this.repository.FindAsync(x => x.Id == id);

            if (item is not null)
            {
                this.repository.Delete(item);
                return Ok();
            }
            else
                return NotFound();
        }
    }
}
