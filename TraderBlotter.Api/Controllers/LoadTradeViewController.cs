using AutoMapper;
using BatchManager.Services;
using DataAccess.Repository;
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
        private readonly ITradeViewRepository _tradeViewRepository;
        private readonly ITradeViewGenericRepository _tradeViewGenericRepository;
        private static ILog _log = LogService.GetLogger(typeof(TradeViewController));
        private readonly ITradeViewNseFoRepository _tradeViewNseFoRepository;
        private readonly ITradeViewNseCmRepository _tradeViewNseCmRepo;
        private readonly ITradeViewBseCmRepository _tradeViewBseCmRepository;

        public LoadTradeViewController(IMapper mapper, ITradeViewRepository tradeViewRepository,
            ITradeViewGenericRepository tradeViewGenericRepository, ITradeViewBseCmRepository tradeViewBseCmRepository, ITradeViewNseFoRepository tradeViewNseFoRepository,
            ITradeViewNseCmRepository tradeViewNseCmRepo)
        {
            _mapper = mapper;
            _tradeViewRepository = tradeViewRepository;
            _tradeViewGenericRepository = tradeViewGenericRepository;
            _tradeViewNseCmRepo = tradeViewNseCmRepo;
            _tradeViewNseFoRepository = tradeViewNseFoRepository;
            _tradeViewBseCmRepository = tradeViewBseCmRepository;
        }

        [HttpGet]
        [Route("syncBseCmTrades")]
        public async Task<IActionResult> SyncBseCmTrades(bool archiveEnabled = false, DateTime dateVal = default(DateTime))
        {
            try
            {
                _log.Info($"SyncBseCmTrades started");
                if (archiveEnabled)
                {
                    var res = await _tradeViewGenericRepository.ArchiveAndPurgeTradeView(Constants.BseCmExchangeName);
                    _log.Info($"Archiving of TradeView is Complete");
                }

                if (dateVal.Equals(default(DateTime)))
                    dateVal = DateTime.Now;

                await _tradeViewBseCmRepository.LoadTradeviewFromSource(dateVal);

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
        [Route("syncNseCmTrades")]
        public async Task<IActionResult> syncNseCmTrades(bool archiveEnabled = false, DateTime dateVal = default(DateTime))
        {
            try
            {
                _log.Info($"syncNseCmTrades started");
                if (archiveEnabled)
                {
                    var res = await _tradeViewGenericRepository.ArchiveAndPurgeTradeView(Constants.NseCmExchangeName);
                    _log.Info($"Archiving of TradeView is Complete");
                }

                if (dateVal.Equals(default(DateTime)))
                    dateVal = DateTime.Now;

                await _tradeViewNseCmRepo.LoadTradeviewFromSource(dateVal);

                _log.Info($"syncNseCmTrades Finished");
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
        public async Task<IActionResult> SyncNseFoTrades(bool archiveEnabled = false, DateTime dateVal = default(DateTime))
        {
            try
            {
                _log.Info($"SyncNseFoTrades started");
                if (archiveEnabled)
                {
                    var res = await _tradeViewGenericRepository.ArchiveAndPurgeTradeView(Constants.NseFoExchangeName);
                    _log.Info($"Archiving of TradeView is Complete");
                }

                if (dateVal.Equals(default(DateTime)))
                    dateVal = DateTime.Now;

                await _tradeViewNseFoRepository.LoadTradeviewFromSource(dateTimeVal:dateVal);
                _log.Info($"SyncNseFoTrades Finished");
                return Ok(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _log.Error("Error in SyncBseCmTrades ", ex);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("SyncBseCmAllTrades")]
        public async Task<IActionResult> SyncBseCmAllTrades()
        {
            try
            {
                _log.Info($"SyncBseCmAllTrades started");
                await _tradeViewBseCmRepository.LoadTradeviewFulDataFromSource();
                _log.Info($"SyncBseCmAllTrades Finished");
                return Ok(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _log.Error("Error in SyncBseCmAllTrades ", ex);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("SyncNseFoAllTrades")]
        public async Task<IActionResult> SyncNseFoAllTrades()
        {
            try
            {
                _log.Info($"SyncNseFoAllTrades started");               
                await _tradeViewNseFoRepository.LoadTradeviewFulDataFromSource();
                _log.Info($"SyncNseFoAllTrades Finished");
                return Ok(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _log.Error("Error in SyncNseFoAllTrades ", ex);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("SyncNseCmAllTrades")]
        public async Task<IActionResult> SyncNseCmAllTrades()
        {
            try
            {
                _log.Info($"SyncNseCmAllTrades started");
                await _tradeViewNseCmRepo.LoadTradeviewFulDataFromSource();
                _log.Info($"SyncNseCmAllTrades Finished");
                return Ok(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _log.Error("Error in SyncNseCmAllTrades ", ex);
                return StatusCode(500);
            }
        }
    }
}
