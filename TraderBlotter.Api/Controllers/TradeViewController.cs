using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Repository.Data;
using DataAccess.Repository.LogServices;
using DataAccess.Repository.Repositories;
using DataAccess.Repository.RepositoryEF.IRepositoryEF;
using log4net;
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
                var resultSet = new List<TradeViewDto>();

                //var tradeDetails = _tradeViewRepository.GetTradeViews();
                //if (tradeDetails == null)
                //    return NotFound();

                //foreach (var item in tradeDetails)
                //{
                //    resultSet.Add(_mapper.Map<TradeViewDto>(item));
                //}

                _log.Info($"In GetAllTrades Started");
                List<TradeView> tradeDetails = null;
                var userDetails = _userViewRepository.GetUserById(userName);
                var role = _roleViewRepository.GetRoles().Where(i => i.RoleId == userDetails.RoleId).FirstOrDefault().RoleName;

                if (role == Roles.SuperAdmin.ToString())
                {
                    var trades = await _tradeViewGenericRepository.GetAllTradeViewsByPageIndex();
                    tradeDetails = trades.ToList();
                }
                else if (role == Roles.GroupUser.ToString())
                {
                    var clientCodes = await _tradeViewGenericRepository.GetClientCodesByGroupName(userDetails.GroupName);
                    var trades = await _tradeViewGenericRepository.GetAllTradeViewsByClientCodes(clientCodes);
                    tradeDetails = trades.ToList();
                }
                else if (role == Roles.Dealer.ToString())
                {
                    var clientCodes = await _tradeViewGenericRepository.GetClientCodesByDealerCode(userDetails.DealerCode);
                    var trades = await _tradeViewGenericRepository.GetAllTradeViewsByClientCodes(clientCodes);
                    tradeDetails = trades.ToList();
                }
                else if (role == Roles.Client.ToString() && !string.IsNullOrWhiteSpace(userDetails.ClientCode))
                {
                    var trades = await _tradeViewGenericRepository.GetAllTradeViewsByClientCodes(new List<string> { userDetails.ClientCode });
                    tradeDetails = trades.ToList();
                }
                else
                {
                    return StatusCode(204, ModelState);
                }

                if (tradeDetails == null)
                    return NotFound();
                foreach (var item in tradeDetails)
                {
                    resultSet.Add(_mapper.Map<TradeViewDto>(item));
                }

                _log.Info($"In GetAllTrades Finished Count-{resultSet.Count}");

                return Ok(resultSet);
            }
            catch (Exception ex)
            {
                _log.Error("Error in GetAllTrades ", ex);
                return StatusCode(500, ModelState);
            }

        }

        private void List<T>()
        {
            throw new NotImplementedException();
        }
    }
}