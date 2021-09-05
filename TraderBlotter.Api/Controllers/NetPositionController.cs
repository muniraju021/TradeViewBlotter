using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Repository.LogServices;
using DataAccess.Repository.Repositories;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TraderBlotter.Api.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class NetPositionController : ControllerBase
    {
        private readonly ITradeViewGenericRepository _tradeViewGenericRepo;
        private static ILog _log = LogService.GetLogger(typeof(NetPositionController));

        public NetPositionController(ITradeViewGenericRepository tradeViewGenericRepo)
        {
            _tradeViewGenericRepo = tradeViewGenericRepo;
        }

        [HttpGet]
        [Route("getNetPositionViewDetails")]
        public async Task<IActionResult> GetNetPositionViewDetails()
        {
            try
            {
                _log.Info($"NetPositionController: GetNetPositionViewDetails Starting..");
                var res = await _tradeViewGenericRepo.GetNetPositionView();
                _log.Info($"NetPositionController: GetNetPositionViewDetails Finished.. Count:{res?.ToList().Count}");
                return Ok(res);
            }
            catch (Exception ex)
            {
                _log.Error("Error in GetNetPositionViewDetails ", ex);
                return StatusCode(500);
            }
        }


    }
}