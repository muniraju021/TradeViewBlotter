using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Repository.Repositories;
using DataAccess.Repository.RepositoryEF.IRepositoryEF;
using Microsoft.AspNetCore.Mvc;
using TraderBlotter.Api.Models.Dto;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace TraderBlotter.Api.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class TradeViewController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITradeViewRepository _tradeViewRepository;
        private readonly ITradeViewGenericRepository _tradeViewGenericRepository;

        public TradeViewController(IMapper mapper, ITradeViewRepository tradeViewRepository,ITradeViewGenericRepository tradeViewGenericRepository)
        {
            _mapper = mapper;
            _tradeViewRepository = tradeViewRepository;
            _tradeViewGenericRepository = tradeViewGenericRepository;
        }

        [HttpGet]
        [Route("getAllTrades")]
        public async Task<IActionResult> GetAllTrades()
        {
            var resultSet = new List<TradeViewDto>();

            //var tradeDetails = _tradeViewRepository.GetTradeViews();
            //if (tradeDetails == null)
            //    return NotFound();

            //foreach (var item in tradeDetails)
            //{
            //    resultSet.Add(_mapper.Map<TradeViewDto>(item));
            //}

            var tradeDetails = await _tradeViewGenericRepository.GetAllTradeViewsByPageIndex(1, 2);
            if (tradeDetails == null)
                return NotFound();
            foreach (var item in tradeDetails)
            {
                resultSet.Add(_mapper.Map<TradeViewDto>(item));
            }

            return Ok(resultSet);
        }
    }
}