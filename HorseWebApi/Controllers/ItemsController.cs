using AutoMapper;
using HorseWebApi.Entities;
using HorseWebApi.Repositories;
using HorseWebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HorseWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IGenericRepository<Item> repository;
        private readonly IMapper mapper;

        public ItemsController(
            IMapper mapper, 
            IGenericRepository<Item> repository)
        {
            this.mapper = mapper ??
                 throw new ArgumentNullException(nameof(mapper));
            this.repository = repository ??
                 throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<Item>>> Get()
        {
            var result = await this.repository.GetAllAsync();

            return result is null
            ? NotFound()
            : Ok(this.mapper.Map<IEnumerable<ItemVM>>(result));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Item>> Get(int id)
        {
            var result = await this.repository.FindAsync(x => x.Id == id);

            return result is null
            ? NotFound()
            : Ok(this.mapper.Map<ItemVM>(result));
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Post([FromBody] ItemVM item)
        {
            await this.repository.AddAsync(this.mapper.Map<Item>(item));

            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Put([FromBody] ItemVM item)
        {
            var result = await this.repository.FindAsync(x => x.Id == item.Id);

            if (result is not null)
            {
                this.repository.Update(this.mapper.Map<Item>(item));
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
