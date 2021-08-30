using DataAccess.Repository.LogServices;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TraderBlotter.Api.Utilities;

namespace TraderBlotter.Api.ConfigurationFilters
{
    public class CustomAuthorizationFilter : IAuthorizationFilter
    {
        private readonly IConfiguration _configuration;
        private static ILog _log = LogService.GetLogger(typeof(CustomAuthorizationFilter));
        public CustomAuthorizationFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var secret = HelperMethods.GetLicensekey();
                var key = Encoding.ASCII.GetBytes(secret);
                var token = context.HttpContext.Request.Headers["Authorization"];

                //var licenseKey = HelperMethods.GetLicensekey();

                if (token.Count > 0 && token.ToString().Contains("Bearer"))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtSecurityToken = handler.ReadJwtToken(token.ToString().Replace("Bearer ", string.Empty));

                    var validatons = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                    SecurityToken tokenSecure = null;
                    var claims = handler.ValidateToken(jwtSecurityToken.RawData, validatons, out tokenSecure);
                    if (tokenSecure == null)
                    {
                        context.Result = new UnauthorizedResult();
                        _log.Error($"CustomAuthorizationFilter: Token couln't be validated..!!!!");
                    }
                    _log.Info($"CustomAuthorizationFilter: Token validation Successfull!!!!");
                }
            }
            catch (Exception ex)
            {
                _log.ErrorFormat($"CustomAuthorizationFilter: Error in Validating Token ",ex);
                context.Result = new UnauthorizedResult();
            }
            
        }
    }
}
