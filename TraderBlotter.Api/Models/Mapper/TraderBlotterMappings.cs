using AutoMapper;
using DataAccess.Repository.Data;
using DataAccess.Repository.Models;
using TraderBlotter.Api.Models.Dto;

namespace TraderBlotter.Api.Models.Mapper
{
    public class TraderBlotterMappings : Profile
    {
        public TraderBlotterMappings()
        {
            CreateMap<TradeView, TradeViewDto>().ReverseMap();
            CreateMap<TradeViewBseCm, TradeView>();
        }
    }
}
