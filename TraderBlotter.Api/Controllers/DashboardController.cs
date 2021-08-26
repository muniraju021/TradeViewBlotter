using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Repository.Infrastructure;
using DataAccess.Repository.LogServices;
using DataAccess.Repository.Repositories;
using DataAccess.Repository.RepositoryEF.IRepositoryEF;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TraderBlotter.Api.Models.Dto;

namespace TraderBlotter.Api.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class DashboardController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITradeViewGenericRepository _tradeViewGenericRepository;
        private static ILog _log = LogService.GetLogger(typeof(DashboardController));
        private readonly IConfiguration _configuration;
        private readonly IConnectionFactory _connectionFactory;

        public DashboardController(IMapper mapper, ITradeViewRepository tradeViewRepository, 
            ITradeViewGenericRepository tradeViewGenericRepository, IConfiguration configuration, IConnectionFactory connectionFactory)
        {
            _mapper = mapper;
            _tradeViewGenericRepository = tradeViewGenericRepository;
            _configuration = configuration;
            _connectionFactory = connectionFactory;
        }

        [HttpGet]
        [Route("getTradesCount")]
        public async Task<IActionResult> GetTradeCounts()
        {
            try
            {
                _log.Info($"DashboardController - In GetTradeCounts");
                var tradstats = await _tradeViewGenericRepository.GetTradeStats();
                var resultSet = _mapper.Map<TradeStatsDto>(tradstats);
                _log.Info($"DashboardController - In GetTradeCounts - Result");
                return Ok(resultSet);
            }
            catch (Exception ex)
            {
                _log.Error("Error in GetTradeCounts ", ex);
                return StatusCode(500, ModelState);
                throw;
            }
        }

        [HttpGet]
        [Route("getHealthCheckStats")]
        public IActionResult GetHealthCheckStats()
        {
            try
            {
                _log.Info($"DashboardController - In getHealthCheckStats");

                using (var con = _connectionFactory.GetConnection())
                {
                    con.Close();
                }

                _log.Info($"DashboardController - In getHealthCheckStats - IsHealthy - OK");
                return Ok(new { IsHealthy = true });

            }
            catch (Exception ex)
            {
                _log.Error("Error in GetTradeCounts ", ex);
                return Ok(new { IsHealthy = false });
                throw;
            }
        }
    }
}