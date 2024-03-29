﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BatchManager.Services;
using DataAccess.Repository;
using DataAccess.Repository.LogServices;
using DataAccess.Repository.Repositories;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;
using TraderBlotter.Api.Models.Dto;

namespace TraderBlotter.Api.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class HealthCheckController : ControllerBase
    {
        private readonly ILog _logger = LogService.GetLogger(typeof(HealthCheckController));
        private readonly ITradeViewGenericRepository _tradeViewGenericRepository;
        private readonly IAutoSyncService _autoSyncService;

        private static bool IsArchived = false;

        //public HealthCheckController(ILoadTradeviewData loadTradeviewData, ITradeViewGenericRepository tradeViewGenericRepository, ILoadTradeviewDataNseFo loadTradeviewDataNseFo)
        public HealthCheckController(ITradeViewGenericRepository tradeViewGenericRepository)
        {
            //_loadTradeviewData = loadTradeviewData;
            //_loadTradeviewDataNseFo = loadTradeviewDataNseFo;
            _tradeViewGenericRepository = tradeViewGenericRepository;
            //_autoSyncService = autoSyncService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                if (!IsArchived)
                {
                    _logger.Info($"HealthCheckController - Archiving Running..");

                    await _tradeViewGenericRepository.ArchiveAndPurgeTradeView(Constants.BseCmExchangeName);
                    _logger.Info($"Archived old days data of - {Constants.BseCmExchangeName}");
                    await _tradeViewGenericRepository.ArchiveAndPurgeTradeView(Constants.NseFoExchangeName);
                    _logger.Info($"Archived old days data of - {Constants.NseFoExchangeName}");
                    await _tradeViewGenericRepository.ArchiveAndPurgeTradeView(Constants.NseCmExchangeName);
                    _logger.Info($"Archived old days data of - {Constants.NseCmExchangeName}");

                    IsArchived = true;
                    _logger.Info($"HealthCheckController - Archiving Complete..");

                }

                //await _loadTradeviewData.LoadBseCmDataFromSourceDb();
                //_logger.Info($"Auto Sync of BSE CM Data Started");

                //await _loadTradeviewDataNseFo.LoadNseFoDataFromSourceDb();
                //_logger.Info($"Auto Sync of NSE FO Data Started");

                //await _autoSyncService.StartAutoSyncFromSource();

                _logger.Info($"HealthCheckController - Returning");
                return Ok(new { IsHealthy = true });
            }
            catch (Exception ex)
            {
                _logger.Error("Auto sync Data Initalization Failed ", ex);
                return StatusCode(500, new ErrorModel { HttpStatusCode = 500, Message = "Auto sync data Intialization Failed" });
            }
            
        }

    }
}