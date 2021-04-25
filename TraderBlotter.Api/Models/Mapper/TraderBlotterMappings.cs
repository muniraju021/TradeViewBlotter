using AutoMapper;
using DataAccess.Repository.Data;
using DataAccess.Repository.Models;
using MySqlX.XDevAPI.CRUD;
using TraderBlotter.Api.Models.Dto;

namespace TraderBlotter.Api.Models.Mapper
{
    public class TraderBlotterMappings : Profile
    {
        public TraderBlotterMappings()
        {
            CreateMap<TradeView, TradeViewDto>().ReverseMap();
            CreateMap<TradeViewBseCm, TradeView>();
            CreateMap<UserView, UserDto>().ReverseMap();
            CreateMap<DealerClientMappingView, DealerClientMappingDto>().ReverseMap();
            CreateMap<GroupDealerMappingView, GroupDealerMappingDto>().ReverseMap();

        }
    }
}
