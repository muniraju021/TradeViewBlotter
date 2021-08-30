using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Repository;
using DataAccess.Repository.Data;
using DataAccess.Repository.LogServices;
using DataAccess.Repository.Repositories;
using DataAccess.Repository.RepositoryEF.IRepositoryEF;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TraderBlotter.Api.Models.Dto;
using TraderBlotter.Api.Utilities;
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
        private readonly IUserViewRepository _userViewRepository;
        private readonly IRoleViewRepository _roleViewRepository;
        private static ILog _log = LogService.GetLogger(typeof(TradeViewController));

        public TradeViewController(IMapper mapper, ITradeViewRepository tradeViewRepository, ITradeViewGenericRepository tradeViewGenericRepository
            , IUserViewRepository userViewRepository, IRoleViewRepository roleViewRepository)
        {
            _mapper = mapper;
            _tradeViewRepository = tradeViewRepository;
            _tradeViewGenericRepository = tradeViewGenericRepository;
            _userViewRepository = userViewRepository;
            _roleViewRepository = roleViewRepository;
        }

        [HttpGet]
        [Route("getAllTrades")]
        public async Task<IActionResult> GetAllTrades(string userName)
        {
            try
            {
                _log.Info($"TradeViewController - In GetAllTrades userName-{userName}");

                var resultSet = new List<TradeViewDto>();

                //var tradeDetails = _tradeViewRepository.GetTradeViews();
                //if (tradeDetails == null)
                //    return NotFound();

                //foreach (var item in tradeDetails)
                //{
                //    resultSet.Add(_mapper.Map<TradeViewDto>(item));
                //}

                _log.Info($"In GetAllTrades Started");
                ConcurrentBag<TradeView> tradeDetails = new ConcurrentBag<TradeView>();
                var userDetails = _userViewRepository.GetUserById(userName);
                var role = _roleViewRepository.GetRoles().Where(i => i.RoleId == userDetails.RoleId).FirstOrDefault().RoleName;

                if (role == Roles.SuperAdmin.ToString())
                {
                    var tradeCount = await _tradeViewGenericRepository.GetAllTradesCount();
                    var iteration = Math.Ceiling(Convert.ToDecimal(tradeCount) / DataAccess.Repository.Constants.ChunkCount);
                    int offset = 0;
                    //for (int i = 0; i < iteration; i++)
                    //{
                    //    _log.Info($"TradeViewController - GetAllTrades userName-{userName} ChunkIndex-[{i}]");
                    //    var trades = await _tradeViewGenericRepository.GetAllTradeViewsByPageIndex(offset);
                    //    tradeDetails.AddRange(trades);
                    //    offset += DataAccess.Repository.Constants.ChunkCount;
                    //}                    
                    //tradeDetails = trades.ToList();

                    var lstOffset = new List<int> { offset };
                    for (int i = 0; i < iteration - 1; i++)
                    {
                        offset += DataAccess.Repository.Constants.ChunkCount;
                        lstOffset.Add(offset);
                    }

                    Parallel.ForEach(lstOffset, offset =>
                    {
                        _log.Info($"TradeViewController - GetAllTrades userName-{userName} ChunkIndex-[{offset}]");
                        var trades = _tradeViewGenericRepository.GetAllTradeViewsByPageIndex(offset);
                        tradeDetails.AddRange<TradeView>(trades.Result);
                    });

                }
                else if (role == Roles.GroupUser.ToString())
                {
                    var clientCodes = await _tradeViewGenericRepository.GetClientCodesByGroupName(userDetails.GroupName);
                    var trades = await _tradeViewGenericRepository.GetAllTradeViewsByClientCodes(clientCodes);
                    tradeDetails.AddRange<TradeView>(trades.ToList());
                }
                else if (role == Roles.Dealer.ToString())
                {
                    var clientCodes = await _tradeViewGenericRepository.GetClientCodesByDealerCode(userDetails.DealerCode);
                    var trades = await _tradeViewGenericRepository.GetAllTradeViewsByClientCodes(clientCodes);
                    tradeDetails.AddRange<TradeView>(trades.ToList());
                }
                else if (role == Roles.Client.ToString() && !string.IsNullOrWhiteSpace(userDetails.ClientCode))
                {
                    var trades = await _tradeViewGenericRepository.GetAllTradeViewsByClientCodes(new List<string> { userDetails.ClientCode });
                    tradeDetails.AddRange<TradeView>(trades.ToList());
                }
                else
                {
                    return StatusCode(204, ModelState);
                }

                //if (tradeDetails == null || tradeDetails.Count == 0)
                //    return NotFound();
                foreach (var item in tradeDetails)
                {
                    resultSet.Add(_mapper.Map<TradeViewDto>(item));
                }

                _log.Info($"TradeViewController - In GetAllTrades Finished Count-{resultSet.Count}");

                return Ok(resultSet);
            }
            catch (Exception ex)
            {
                _log.Error("Error in GetAllTrades ", ex);
                return StatusCode(500, ModelState);
            }

        }

        [HttpGet]
        [Route("getAllTradesCount")]
        public async Task<IActionResult> GetAllTradesCount()
        {
            try
            {
                _log.Info($"TradeViewController - In GetAllTradesCount - Starting..");
                var res = await _tradeViewGenericRepository.GetAllTradesCount();
                _log.Info($"TradeViewController - In GetAllTradesCount - Finished.. -Count:{res}");
                return Ok(res);

            }
            catch (Exception ex)
            {
                _log.Error("Error in GetAllTradesCount ", ex);
                return StatusCode(500, ModelState);
            }
        }

    }
}