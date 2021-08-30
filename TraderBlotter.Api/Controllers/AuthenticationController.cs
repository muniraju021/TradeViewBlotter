using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Repository.Models;
using DataAccess.Repository.RepositoryEF.IRepositoryEF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TraderBlotter.Api.Models.Dto;

namespace TraderBlotter.Api.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserViewRepository _userViewRepository;
        private readonly IConfiguration _configuration;
        private readonly IRoleViewRepository _roleViewRepository;
        private readonly IMapper _mapper;

        public AuthenticationController(IUserViewRepository userViewRepository, IRoleViewRepository roleViewRepository, IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _userViewRepository = userViewRepository;
            _roleViewRepository = roleViewRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody]LoginRequestDto model)
        {
            var user = _userViewRepository.ValidateLogin(model.LoginName, model.Password);

            if(user != null)
            {
                var role = _roleViewRepository.GetRoleById(user.RoleId);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.LoginName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role,role.RoleName)
                };
                
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                       //issuer: _configuration["JWT:ValidIssuer"],
                       //audience: _configuration["JWT:ValidAudience"],
                       expires: DateTime.Now.AddHours(10),
                       claims: authClaims,
                       signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
                    );

                var userDto = _mapper.Map<UserDto>(user);
                userDto.Token = new JwtSecurityTokenHandler().WriteToken(token);
                userDto.TokenExpiration = token.ValidTo;

                return Ok(userDto);
            }

            return Unauthorized();
        }
    }
}