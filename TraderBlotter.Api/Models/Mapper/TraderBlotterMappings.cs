using AutoMapper;
using DataAccess.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TraderBlotter.Api.Data;
using TraderBlotter.Api.Models.Dto;

namespace TraderBlotter.Api.Models.Mapper
{
    public class TraderBlotterMappings : Profile
    {
        public TraderBlotterMappings()
        {
            CreateMap<TradeView, TradeViewDto>().ReverseMap();
        }
    }
}
