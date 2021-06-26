using AutoMapper;
using DataAccess.Repository.LogServices;
using DataAccess.Repository.Models;
using DataAccess.Repository.RepositoryEF.IRepositoryEF;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private static ILog _log = LogService.GetLogger(typeof(UserManagementController));

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
                return StatusCode(500, new ErrorModel { HttpStatusCode = 500, Message = "Internal Server Error" });
            }

        }

        [HttpPost]
        [Route("validateLogin")]
        public IActionResult ValidateLogin([FromBody] LoginRequestDto loginRequest)
        {
            try
            {
                _log.Info($"ValidateLogin Callling: {loginRequest?.LoginName}");

                var user = _userViewRepository.ValidateLogin(loginRequest.LoginName, loginRequest.Password);
                if (user == null)
                    return StatusCode(401, new ErrorModel { HttpStatusCode = 401, Message = "Invalid UserName and Password" });

                var userDto = _mapper.Map<UserDto>(user);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _log.Error($"Error in ValidateLogin - ", ex);
                return StatusCode(500, new ErrorModel { HttpStatusCode = 500, Message = "Internal Server Error" });
            }
            
        }

        [HttpGet]
        [Route("getUserByLoginName")]
        public IActionResult GetUserInfo(string loginName)
        {
            try
            {
                _log.Info($"GetUserInfo Callling: {loginName}");

                var obj = _userViewRepository.GetUserById(loginName);
                if (obj == null)
                    return StatusCode(401, new ErrorModel { HttpStatusCode = 401, Message = "User Not Found" });
                var userDto = _mapper.Map<UserDto>(obj);

                if (!string.IsNullOrWhiteSpace(userDto.GroupName))
                    userDto.RoleCode = userDto.GroupName;
                else if (!string.IsNullOrWhiteSpace(userDto.DealerCode))
                    userDto.RoleCode = userDto.DealerCode;
                else if (!string.IsNullOrWhiteSpace(userDto.ClientCode))
                    userDto.RoleCode = userDto.ClientCode;

                userDto.RoleName = _roleViewRepository.GetRoleById(userDto.RoleId)?.RoleName;

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _log.Error($"Error in GetUserInfo - ", ex);
                return StatusCode(500, new ErrorModel { HttpStatusCode = 500, Message = "Internal Server Error" });
            }
            
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
            _log.Info($"GetClientCodes Calling");
            return Ok(_userViewRepository.GetClientViews());
        }

        [HttpGet]
        [Route("getGroups")]
        public IActionResult GetGroups()
        {
            _log.Info($"GetGroups Calling");
            return Ok(_userViewRepository.GetGroups());
        }

        [HttpGet]
        [Route("getDealersByGroupName")]
        public IActionResult GetDealersByGroupName(string groupName)
        {
            try
            {
                _log.Info($"GetDealersByGroupName Calling - {groupName} ");
                var lst = _userViewRepository.GetDealersByGroupName(groupName);
                return Ok(lst);
            }
            catch (Exception ex)
            {
                _log.Error($"Error in GetDealersByGroupName - ", ex);
                return StatusCode(500, new ErrorModel { HttpStatusCode = 500, Message = "Internal Server Error" });
            }            
        }

        [HttpGet]
        [Route("getDealerNotMappedToGroupName")]
        public IActionResult GetDealerNotMappedToGroupName(string groupName)
        {
            try
            {
                _log.Info($"GetDealerNotMappedToGroupName Calling - {groupName} ");
                var lst = _userViewRepository.GetDealersNotMappedToGroupName(groupName);
                return Ok(lst);
            }
            catch (Exception ex)
            {
                _log.Error($"Error in GetDealerNotMappedToGroupName - ", ex);
                return StatusCode(500, new ErrorModel { HttpStatusCode = 500, Message = "Internal Server Error" });
            }
        }

        [HttpGet]
        [Route("getClientCodeByDealerCode")]
        public IActionResult GetClientCodeByDealerCode(string dealerCode)
        {
            try
            {
                _log.Info($"GetClientCodeByDealerCode Calling - {dealerCode} ");
                var lst = _userViewRepository.GetClientCodesByDealerCode(dealerCode);
                return Ok(lst);
            }
            catch (Exception ex)
            {
                _log.Error($"Error in GetClientCodeByDealerCode - ", ex);
                return StatusCode(500, new ErrorModel { HttpStatusCode = 500, Message = "Internal Server Error" });
            }

        }

        [HttpGet]
        [Route("getClientCodesNotMappedToDealerCode")]
        public IActionResult GetClientCodesNotMappedToDealerCode(string dealerCode)
        {
            try
            {
                _log.Info($"GetClientCodesNotMappedToDealerCode Calling - {dealerCode} ");
                var lst = _userViewRepository.GetClientCodesNotMappedToDealerCode(dealerCode);
                _log.Info($"GetClientCodesNotMappedToDealerCode: DealerCode:{dealerCode} UnMappedClients:{lst.Count}");
                return Ok(lst);
            }
            catch (Exception ex)
            {
                _log.Error($"Error in GetClientCodesNotMappedToDealerCode - ", ex);
                return StatusCode(500, new ErrorModel { HttpStatusCode = 500, Message = "Internal Server Error" });
            }
        }

        [HttpGet]
        [Route("getUserCodes")]
        public IActionResult GetUserCodes(string role)
        {
            var roleId = _roleViewRepository.GetRoles().Where(i => i.RoleName.ToUpper() == role.ToUpper())?.Select(j => j.RoleId).FirstOrDefault();
            if (roleId == null || roleId == 0)
                return StatusCode(400, new ErrorModel { Message = "Invalid Role", HttpStatusCode = 400 });

            switch (roleId)
            {
                case (int)Roles.GroupUser:
                    return Ok(_userViewRepository.GetGroups());
                case (int)Roles.Dealer:
                    return Ok(_userViewRepository.GetDealers());
                case (int)Roles.Client:
                    return Ok(_userViewRepository.GetClientViews());
                default:
                    return StatusCode(400, new ErrorModel { Message = "Invalid Role", HttpStatusCode = 400 });
            }

        }

        [HttpGet]
        [Route("getUsers")]
        public async Task<IActionResult> GetUsersAsync()
        {
            try
            {
                var users = _userViewRepository.GetUserViews();
                var usersDto = new List<UserDto>();
                foreach (var item in users)
                {
                    var obj = _mapper.Map<UserDto>(item);
                    var roleName = (Roles)obj.RoleId;
                    obj.RoleName = roleName.ToString();

                    if (!string.IsNullOrWhiteSpace(item.GroupName))
                        obj.RoleCode = item.GroupName;
                    else if (!string.IsNullOrWhiteSpace(item.DealerCode))
                        obj.RoleCode = item.DealerCode;
                    else if (!string.IsNullOrWhiteSpace(item.ClientCode))
                        obj.RoleCode = item.ClientCode;

                    usersDto.Add(obj);                    

                }
                return Ok(usersDto);
            }
            catch (Exception ex)
            {
                _log.Error($"Error in GetUserAsync ", ex);
                return StatusCode(500, new ErrorModel { Message = ex.Message, HttpStatusCode = 500 });
            }
        }
            


        [HttpPost]
        [Route("addUser")]
        public async Task<IActionResult> AddUserAsync([FromBody] UserRequestDto userRequest)
        {
            try
            {
                if (userRequest != null)
                {
                    var userView = new UserView
                    {
                        LoginName = userRequest.LoginName,
                        Password = userRequest.Password,
                        EmailId = userRequest.EmailId,
                        IsActive = userRequest.IsActive
                    };

                    var roleId = _roleViewRepository.GetRoles().Where(i => i.RoleName == userRequest.RoleName)?.Select(j => j.RoleId).FirstOrDefault();
                    if (roleId == null || roleId == 0)
                        return StatusCode(400, new ErrorModel { Message = "Invalid Role", HttpStatusCode = 400 });
                    userView.RoleId = roleId.Value;

                    if (!string.IsNullOrWhiteSpace(userRequest.UserCode))
                    {
                        if (userRequest.RoleName == Roles.GroupUser.ToString())
                            userView.GroupName = userRequest.UserCode;
                        else if (userRequest.RoleName == Roles.Dealer.ToString())
                            userView.DealerCode = userRequest.UserCode;
                        else if (userRequest.RoleName == Roles.Client.ToString())
                            userView.ClientCode = userRequest.UserCode;
                    }

                    if (!string.IsNullOrWhiteSpace(userView.ClientCode) && !_userViewRepository.GetClientViews().Select(i => i.ClientCode).Contains(userView.ClientCode))
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
                return StatusCode(500, new ErrorModel { Message = ex.Message, HttpStatusCode = 500 });
            }
        }

        [HttpPost]
        [Route("updateUser")]
        public IActionResult UpdateUserAsync([FromBody] UserRequestDto userRequest)
        {
            try
            {
                if (userRequest != null)
                {
                    var userView = new UserView
                    {
                        LoginName = userRequest.LoginName,
                        Password = userRequest.Password,
                        EmailId = userRequest.EmailId,
                        IsActive = userRequest.IsActive
                    };

                    var roleId = _roleViewRepository.GetRoles().Where(i => i.RoleName == userRequest.RoleName)?.Select(j => j.RoleId).FirstOrDefault();
                    if (roleId == null || roleId == 0)
                        return StatusCode(400, new ErrorModel { Message = "Invalid Role", HttpStatusCode = 400 });
                    userView.RoleId = roleId.Value;

                    if (!string.IsNullOrWhiteSpace(userRequest.UserCode))
                    {
                        if (userRequest.RoleName == Roles.GroupUser.ToString())
                            userView.GroupName = userRequest.UserCode;
                        else if (userRequest.RoleName == Roles.Dealer.ToString())
                            userView.DealerCode = userRequest.UserCode;
                        else if (userRequest.RoleName == Roles.Client.ToString())
                            userView.ClientCode = userRequest.UserCode;
                    }

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
                return StatusCode(500, new ErrorModel { Message = ex.Message, HttpStatusCode = 500 }); ;
            }

        }

        [HttpPost]
        [Route("deleteUser")]
        public IActionResult DeleteUserAsync([FromBody] UserView userview)
        {
            try
            {
                if (userview != null)
                {

                    _userViewRepository.DeleteUser(userview);
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
    }
}
