using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using Nest;
using System.Threading.Tasks;
using XHTDHP_API.Bussiness;
using Microsoft.Extensions.Configuration;
using System;
using XHTDHP_API.Auth;
using XHTDHP_API.Models;
using XHTDHP_API.ModelsIdentity;
using System.Collections.Generic;
using XHTDHP_API.Logging;

namespace XHTDHP_API.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class AccountController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IConfiguration _configuration;
        Function objFunction = new Function();
        //public AccountController(IConfiguration configuration, ILoggerManager logger)
        //{
        //    _configuration = configuration;
        //    _logger = logger;
        //    objFunction = new Function(configuration);
        //}

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("/token")]
        public async Task<ActionResult<Object>> token([FromForm] LoginDto model)
        {
            var responseModel = new SumProfileResponseDTO();
            if (String.IsNullOrEmpty(model.username) || String.IsNullOrEmpty(model.password))
            {
                return responseModel;
            }
            else
            {
                var checkUserNameAndPass = objFunction.checkUserNameAndPassWord(model.username, model.password);
                if (!checkUserNameAndPass) return responseModel;
                var user = new AppUser
                {
                    UserName = model.username,
                    NormalizedUserName = model.username,
                    PasswordHash = model.password
                };
                if (user != null)
                {
                    var jwt = await GenerateJwtToken(user);
                    var expireIn = Convert.ToDouble(3600);
                    responseModel.expires_in = expireIn;
                    responseModel.access_token = jwt.ToString();
                    responseModel.token_type = "bearer";
                    return responseModel;
                }
                else
                {
                    return responseModel;
                }
            }
        }
        private async Task<string> GenerateJwtToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretsecretsecretsecret"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddSeconds(3600);

            var token = new JwtSecurityToken(
                "tmp",
                "tmp",
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        //[HttpPost]
        //[Route("api/Login")]
        //[Authorize]
        //public Account Login(Account objAccountLogin)
        //{

        //}
    }
}
