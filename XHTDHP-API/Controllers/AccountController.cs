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
using XHTDHP_API.Entities;
using XHTDHP_API.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace XHTDHP_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class AccountController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IConfiguration _configuration;
        private readonly ApiDbContext _context;
        public AccountController(ApiDbContext context)
        {
            _context = context;
        }
        Function objFunction = new Function();
        //public AccountController(IConfiguration configuration, ILoggerManager logger)
        //{
        //    _configuration = configuration;
        //    _logger = logger;
        //    objFunction = new Function(configuration);
        //}

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public async Task<ActionResult<Object>> Login([FromBody] LoginDto model)
        {
            var responseModel = new SumProfileResponseDTO();
            responseModel.ListRole = new List<string>();
            
            //TODO: remove sau khi done phần qly tài khoản
            responseModel.ListRole.Add("Admin");
            responseModel.ListRole.Add("ds");
            
            if (String.IsNullOrEmpty(model.userName) || String.IsNullOrEmpty(model.password))
            {
                return responseModel;
            }
            else
            {
                var checkUserNameAndPass = objFunction.checkUserNameAndPassWord(model.userName, model.password);
                var account = _context.tblAccount.FirstOrDefault(u => u.UserName == model.userName);
                // var isPasswordMatched = VerifyPassword(model.password, model.StoredSalt, account.Password);
                if (!checkUserNameAndPass) return responseModel;
                
                var user = new AppUser
                {
                    UserName = model.userName,
                    NormalizedUserName = model.userName,
                    PasswordHash = model.password
                };

                if (user != null)
                {
                    // var lstRole = await _userManager.GetRolesAsync(user);
                    // responseModel.ListRole = _mapper.Map<List<string>>(lstRole);
                    var jwt = await GenerateJwtToken(user);
                    var expireIn = Convert.ToDouble(3600);
                    responseModel.expires_in = expireIn;
                    responseModel.access_token = jwt.ToString();
                    responseModel.token_type = "bearer";
                    responseModel.errorCode = "200";
                    return responseModel;
                } else {
                    return responseModel;
                }
            }
        }

        [HttpPost]
        [Route("/register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            if (String.IsNullOrEmpty(model.UserName))
            {
                return BadRequest("Vui lòng nhập username");
            }
            if (String.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Vui lòng nhập password");
            }
            var accountExist = await _context.tblAccount.Where(item => item.UserName == model.UserName).FirstOrDefaultAsync();
            if (accountExist != null)
            {
                return Ok(accountExist);
            }
            var hashsalt = EncryptPassword(model.Password);
            model.Password = hashsalt.Hash;
            
            // model.StoredSalt = hashsalt.Salt;
            var newAccount = new tblAccount 
            {
                UserName = model.UserName,
                Password = model.Password,
                GroupId = 1,
                State = true,
                CreateDay = DateTime.Now
            };
            await _context.tblAccount.AddAsync(newAccount);
            await _context.SaveChangesAsync();
            return Ok(hashsalt.Salt);
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

        private HashSalt EncryptPassword(string password)
        {
            byte[] salt = new byte[128 / 8]; // Generate a 128-bit salt using a secure PRNG
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string encryptedPassw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));
            return new HashSalt { Hash = encryptedPassw , Salt = salt };
        }
            
        private bool VerifyPassword(string enteredPassword, byte[] salt, string storedPassword)
        {
            string encryptedPassw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));
            return encryptedPassw == storedPassword;
        }
    }
}
