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
            CreateMap<TradeViewNseFo, TradeView>();

            CreateMap<TradeViewBseCm, TradeViewRef>();
            CreateMap<TradeViewNseFo, TradeViewRef>();
            CreateMap<TradeViewNseCm, TradeViewRef>();

            CreateMap<UserView, UserDto>().ReverseMap();
            CreateMap<DealerClientMappingView, DealerClientMappingDto>().ReverseMap();
            CreateMap<GroupDealerMappingView, GroupDealerMappingDto>().ReverseMap();

            CreateMap<TradeStats, TradeStatsDto>();
        }
    }
}
