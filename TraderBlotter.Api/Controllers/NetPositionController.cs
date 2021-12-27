using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Repository.LogServices;
using DataAccess.Repository.Models;
using DataAccess.Repository.Repositories;
using DataAccess.Repository.RepositoryEF.IRepositoryEF;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using TraderBlotter.Api.Utilities;

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
        private readonly IRoleViewRepository _roleRepository;
        private readonly IUserViewRepository _userViewRepository;
        private readonly IGroupDealerMappingRepository _groupDealerMappingRepository;

        public NetPositionController(ITradeViewGenericRepository tradeViewGenericRepo, IRoleViewRepository roleRepository, 
            IUserViewRepository userViewRepository, IGroupDealerMappingRepository groupDealerMappingRepository)
        {
            _tradeViewGenericRepo = tradeViewGenericRepo;
            _roleRepository = roleRepository;
            _userViewRepository = userViewRepository;
            _groupDealerMappingRepository = groupDealerMappingRepository;
        }

        [HttpGet]
        [Route("getNetPositionViewDetails")]
        public async Task<IActionResult> GetNetPositionViewDetails(string userName)
        {
            try
            {
                var userDetails = _userViewRepository.GetUserById(userName);
                var role = _roleRepository.GetRoles().Where(i => i.RoleId == userDetails.RoleId).FirstOrDefault().RoleName;
                _log.Info($"NetPositionController: GetNetPositionViewDetails Starting.. User: {userName} Role: {role}");

                var res = new List<NetPositionView>();

                if (role == Roles.SuperAdmin.ToString())
                {
                    res = (await _tradeViewGenericRepo.GetNetPositionView())?.ToList();
                }
                else if(role == Roles.Dealer.ToString())
                {
                    #region comment
                    //var clientCodes = await _tradeViewGenericRepo.GetClientCodesByDealerCode(userDetails.DealerCode);
                    //if(clientCodes?.Count > 0)
                    //{
                    //    res = (await _tradeViewGenericRepo.GetNetPositionViewByDealerClients(new List<string> { userDetails.DealerCode }, clientCodes)).ToList();
                    //}
                    #endregion

                    var dealerCodes = new List<string> { userDetails.DealerCode };
                    if(dealerCodes?.Count > 0)
                    {
                        res = (await _tradeViewGenericRepo.GetNetPoistionViewByDealerCodes(dealerCodes)).ToList();
                    }

                }
                else if(role == Roles.GroupUser.ToString())
                {
                    #region comment
                    //var clientCodes = await _tradeViewGenericRepo.GetClientCodesByGroupName(userDetails.GroupName);
                    //if (clientCodes?.Count > 0)
                    //{
                    //    res = (await _tradeViewGenericRepo.GetNetPositionViewByClients(clientCodes)).ToList();
                    //}
                    #endregion

                    var dealerCodes = _groupDealerMappingRepository.GetDealerByGroupName(userDetails.GroupName).ToList();
                    if (dealerCodes?.Count > 0)
                    {
                        res = (await _tradeViewGenericRepo.GetNetPoistionViewByDealerCodes(dealerCodes)).ToList();
                    }

                }
                else if(role == Roles.Client.ToString())
                {
                    res = (await _tradeViewGenericRepo.GetNetPositionViewByClients(new List<string> { userDetails.ClientCode })).ToList();
                }
                               
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