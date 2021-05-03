using AutoMapper;
using BatchManager.Services;
using DataAccess.Repository.LogServices;
using DataAccess.Repository.Repositories;
using DataAccess.Repository.RepositoryEF.IRepositoryEF;
using log4net;
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
        private readonly ITradeViewRepository _tradeViewRepository;
        private readonly ITradeViewGenericRepository _tradeViewGenericRepository;
        private static ILog _log = LogService.GetLogger(typeof(TradeViewController));
        private readonly ITradeViewNseFoRepository _tradeViewNseFoRepository;

        public LoadTradeViewController(IMapper mapper, ITradeViewBseCmRepository tradeViewBseCmRepository, ITradeViewRepository tradeViewRepository,
            ITradeViewGenericRepository tradeViewGenericRepository, ITradeViewNseFoRepository tradeViewNseFoRepository)
        {
            _mapper = mapper;
            _tradeViewBseCmRepository = tradeViewBseCmRepository;
            _tradeViewRepository = tradeViewRepository;
            _tradeViewGenericRepository = tradeViewGenericRepository;
            _tradeViewNseFoRepository = tradeViewNseFoRepository;
        }

        [HttpGet]
        [Route("syncBseCmTrades")]
        public async Task<IActionResult> SyncBseCmTrades(bool archiveEnabled = false)
        {
            try
            {
                _log.Info($"SyncBseCmTrades started");
                if (archiveEnabled)
                {
                    var res = await _tradeViewGenericRepository.ArchiveAndPurgeTradeView();
                    _log.Info($"Archiving of TradeView is Complete");
                }

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

        [HttpGet]
        [Route("syncNseFoTrades")]
        public async Task<IActionResult> SyncNseFoTrades(bool archiveEnabled = false)
        {
            try
            {
                _log.Info($"SyncNseFoTrades started");
                if (archiveEnabled)
                {
                    var res = await _tradeViewGenericRepository.ArchiveAndPurgeTradeView();
                    _log.Info($"Archiving of TradeView is Complete");
                }

                await _tradeViewNseFoRepository.LoadTradeviewFromSource();
                _log.Info($"SyncNseFoTrades Finished");
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
