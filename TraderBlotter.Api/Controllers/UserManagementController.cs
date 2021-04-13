using AutoMapper;
using DataAccess.Repository.Models;
using DataAccess.Repository.RepositoryEF.IRepositoryEF;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TraderBlotter.Api.Models.Dto;
using TraderBlotter.Api.Utilities;

namespace TraderBlotter.Api.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserViewRepository _userViewRepository;
        private readonly IRoleViewRepository _roleViewRepository;
        private readonly IMapper _mapper;
        private static ILog _log = Logger.GetLogger(typeof(UserManagementController));

        public UserManagementController(IUserViewRepository userViewRepository, IRoleViewRepository roleViewRepository, IMapper mapper)
        {
            _userViewRepository = userViewRepository;
            _roleViewRepository = roleViewRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("getAllRoles")]
        public IActionResult GetRoles()
        {
            try
            {
                var roles = _roleViewRepository.GetRoles();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                _log.Error($"Error in GetRoles - ", ex);
                return StatusCode(500, new ErrorModel { HttpStatusCode = 500,Message = "Internal Server Error" });
            }
            
        }

        [HttpPost]
        [Route("validateLogin")]
        public IActionResult ValidateLogin([FromBody]LoginRequestDto loginRequest)
        {
            var user = _userViewRepository.ValidateLogin(loginRequest.LoginName, loginRequest.Password);
            if (user == null)
                return StatusCode(401, new ErrorModel { HttpStatusCode = 401, Message = "Invalid UserName and Password" });

            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [HttpGet]
        [Route("getDealers")]
        public IActionResult GetDealers()
        {
            return Ok(_userViewRepository.GetDealers());
        }

        [HttpGet]
        [Route("getClientCodes")]
        public IActionResult GetClientCodes()
        {
            return Ok(_userViewRepository.GetClientViews());
        }

        [HttpGet]
        [Route("getGroups")]
        public IActionResult GetGroups()
        {
            return Ok(_userViewRepository.GetGroups());
        }

        [HttpPost]
        [Route("addUser")]
        public async Task<IActionResult> AddUserAsync([FromBody]UserView userView)
        {
            try
            {
                if (userView != null)
                {
                    userView.ClientCode = string.IsNullOrWhiteSpace(userView.ClientCode) ? null : userView.ClientCode;
                    userView.GroupName = string.IsNullOrWhiteSpace(userView.GroupName) ? null : userView.GroupName;
                    userView.DealerCode = string.IsNullOrWhiteSpace(userView.DealerCode) ? null : userView.DealerCode;

                    if (!_roleViewRepository.GetRoles().Select(i => i.RoleId).Contains(userView.RoleId))
                        return StatusCode(400, new ErrorModel { Message = "Invalid Role", HttpStatusCode = 400 });
                    if(!string.IsNullOrWhiteSpace(userView.ClientCode) && !_userViewRepository.GetClientViews().Select(i => i.ClientCode).Contains(userView.ClientCode))
                        return StatusCode(400, new ErrorModel { Message = "Invalid ClientCode", HttpStatusCode = 400 });
                    if (!string.IsNullOrWhiteSpace(userView.GroupName) && !_userViewRepository.GetGroups().Select(i => i.GroupName).Contains(userView.GroupName))
                        return StatusCode(400, new ErrorModel { Message = "Invalid GroupCode", HttpStatusCode = 400 });
                    if (!string.IsNullOrWhiteSpace(userView.DealerCode) && !_userViewRepository.GetDealers().Select(i => i.DealerCode).Contains(userView.DealerCode))
                        return StatusCode(400, new ErrorModel { Message = "Invalid DealerCode", HttpStatusCode = 400 });

                    await _userViewRepository.AddUserAsync(userView);
                    return Ok();
                }
                else
                    return StatusCode(400, new ErrorModel { Message = "Bad Parameter Passed", HttpStatusCode = 400 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel { Message = ex.Message, HttpStatusCode = 500 }); ;
            }
            
        }

        [HttpPost]
        [Route("updateUser")]
        public IActionResult UpdateUserAsync([FromBody]UserView userView)
        {
            try
            {
                if (userView != null)
                {
                    userView.ClientCode = string.IsNullOrWhiteSpace(userView.ClientCode) ? null : userView.ClientCode;
                    userView.GroupName = string.IsNullOrWhiteSpace(userView.GroupName) ? null : userView.GroupName;
                    userView.DealerCode = string.IsNullOrWhiteSpace(userView.DealerCode) ? null : userView.DealerCode;

                    if (!_roleViewRepository.GetRoles().Select(i => i.RoleId).Contains(userView.RoleId))
                        return StatusCode(400, new ErrorModel { Message = "Invalid Role", HttpStatusCode = 400 });
                    if (!string.IsNullOrWhiteSpace(userView.ClientCode) && !_userViewRepository.GetClientViews().Select(i => i.ClientCode).Contains(userView.ClientCode))
                        return StatusCode(400, new ErrorModel { Message = "Invalid ClientCode", HttpStatusCode = 400 });
                    if (!string.IsNullOrWhiteSpace(userView.GroupName) && !_userViewRepository.GetGroups().Select(i => i.GroupName).Contains(userView.GroupName))
                        return StatusCode(400, new ErrorModel { Message = "Invalid GroupCode", HttpStatusCode = 400 });
                    if (!string.IsNullOrWhiteSpace(userView.DealerCode) && !_userViewRepository.GetDealers().Select(i => i.DealerCode).Contains(userView.DealerCode))
                        return StatusCode(400, new ErrorModel { Message = "Invalid DealerCode", HttpStatusCode = 400 });

                    _userViewRepository.UpdateUserAsync(userView);
                    return Ok();
                }
                else
                    return StatusCode(400, new ErrorModel { Message = "Bad Parameter Passed", HttpStatusCode = 400 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel { Message = ex.Message, HttpStatusCode = 500, ExceptionObj = ex }); ;
            }

        }
    }
}
