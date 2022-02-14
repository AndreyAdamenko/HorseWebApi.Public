using HorseWebApi.Entities;
using HorseWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HorseWebApi.Infrastructure
{
    public static class HorseMapper
    {
        public static OrderVM Map(Order order)
        {
            return new OrderVM
            {
                Id = order.Id,
                Date = order.Date,
                Note = order.Note,
                TotalPrice = order.Items.Select(y => y.Item.Price * y.Count).Sum(),
                UserId = order.User is null ? order.User.Id : 0,
                Items = order.Items.Select(y => new ItemVM
                {
                    Id = y.Item.Id,
                    Count = y.Count,
                    Name = y.Item.Name,
                    Price = y.Item.Price,
                    Weight = y.Item.Weight
                }).ToList()
            };
        }
    }
}
