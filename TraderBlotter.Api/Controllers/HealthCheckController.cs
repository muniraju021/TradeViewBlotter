using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BatchManager.Services;
using DataAccess.Repository.LogServices;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraderBlotter.Api.Models.Dto;

namespace TraderBlotter.Api.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class HealthCheckController : ControllerBase
    {
        private static ILoadTradeviewData _loadTradeviewData;
        private readonly ILog _logger = LogService.GetLogger(typeof(HealthCheckController));
        public HealthCheckController(ILoadTradeviewData loadTradeviewData)
        {
            _loadTradeviewData = loadTradeviewData;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                await _loadTradeviewData.LoadBseCmDataFromSourceDb();
                _logger.Info($"HealthCheckController - Returning");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error("Auto sync Data Initalization Failed ", ex);
                return StatusCode(500, new ErrorModel { HttpStatusCode = 500, Message = "Auto sync data Intialization Failed" });
            }
            
        }

    }
}