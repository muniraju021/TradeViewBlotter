using AutoMapper;
using DataAccess.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TraderBlotter.Api.Models.Dto;

namespace TraderBlotter.Api.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class LoadTradeViewController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITradeViewBseCmRepository _tradeViewBseCmRepository;

        public LoadTradeViewController(IMapper mapper, ITradeViewBseCmRepository tradeViewBseCmRepository)
        {
            _mapper = mapper;
            _tradeViewBseCmRepository = tradeViewBseCmRepository;
        }

        [HttpGet]
        [Route("syncBseCmTrades")]
        public async Task<IActionResult> SyncBseCmTrades()
        {
            await _tradeViewBseCmRepository.LoadTradeviewFromSource();
            return Ok(HttpStatusCode.OK);
        }

    }
}
