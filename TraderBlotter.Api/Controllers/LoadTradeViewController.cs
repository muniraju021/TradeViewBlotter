using AutoMapper;
using DataAccess.Repository.Repositories;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TraderBlotter.Api.Models.Dto;
using TraderBlotter.Api.Utilities;

namespace TraderBlotter.Api.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class LoadTradeViewController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITradeViewBseCmRepository _tradeViewBseCmRepository;
        private static ILog _log = Logger.GetLogger(typeof(TradeViewController));

        public LoadTradeViewController(IMapper mapper, ITradeViewBseCmRepository tradeViewBseCmRepository)
        {
            _mapper = mapper;
            _tradeViewBseCmRepository = tradeViewBseCmRepository;
        }

        [HttpGet]
        [Route("syncBseCmTrades")]
        public async Task<IActionResult> SyncBseCmTrades()
        {
            try
            {
                _log.Info($"SyncBseCmTrades started");
                await _tradeViewBseCmRepository.LoadTradeviewFromSource();
                _log.Info($"SyncBseCmTrades Finished");
                return Ok(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _log.Error("Error in SyncBseCmTrades ", ex);
                return StatusCode(500);
            }
            
        }

    }
}
