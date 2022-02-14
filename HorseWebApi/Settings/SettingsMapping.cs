using System.Linq;

using AutoMapper;

using HorseWebApi.Entities;
using HorseWebApi.Requests;
using HorseWebApi.ViewModels;

namespace HorseWebApi.Settings
{
    public class SettingsMapping : Profile
    {
        public SettingsMapping()
        {
            CreateMap<Region, RegionVM>().ReverseMap();
            CreateMap<Region, RegionRequest>().ReverseMap();

            CreateMap<Item, ItemVM>().ReverseMap();

            CreateMap<OrderItemRequest, OrderItem>().ReverseMap();
            CreateMap<OrderRequest, Order>()
                .ForMember(x => x.User, c => c.MapFrom(y => new User { Id = y.UserId }))
                .ForMember(x => x.Items, c => c.MapFrom(y => y.ItemIds.Select(z => new OrderItem { Item = new Item { Id = z.ItemId }, Count = z.Count })))
                .ForMember(x => x.Region, c => c.MapFrom(y => new Region { Id = y.RegionId }))
                .ReverseMap();
        }
    }
}
