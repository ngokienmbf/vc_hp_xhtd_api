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
using XHTDHP_API.Models.Filter;
using XHTDHP_API.Helpers;

namespace XHTDHP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
                return BadRequest("Tài khoản đã tồn tại");
            }
            var hashPassord = objFunction.CryptographyMD5(model.Password);
            
            // model.StoredSalt = hashsalt.Salt;
            var newAccount = new tblAccount 
            {
                UserName = model.UserName,
                Password = hashPassord,
                GroupId = 1,
                State = true,
                CreateDay = DateTime.Now
            };
            await _context.tblAccount.AddAsync(newAccount);
            await _context.SaveChangesAsync();
            return Ok(new { statusCode = 200, message = "Đăng ký tài khoản thành công"});
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

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var query = _context.tblAccount.OrderBy(item => item.UserName).AsNoTracking();
            if (!String.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(item => item.FullName.Contains(filter.Keyword) || item.UserName.Contains(filter.Keyword));
            }
            var totalRecords = await query.CountAsync();
            query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            var pagedData = await query.ToListAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<tblAccount>(pagedData, filter, totalRecords);
            return Ok(pagedReponse);
        }
        
        [HttpGet("GetFull")]
        public async Task<IActionResult> GetFull()
        {
            var query = await _context.tblAccount.OrderBy(item => item.UserName).ToListAsync();
            return Ok(query);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var found = await _context.tblAccount.FindAsync(id);
            return Ok(found);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] tblAccount model)
        {
            model.UpdateDay = DateTime.Now;
            
            if (!String.IsNullOrEmpty(model.Password))
            {
                model.Password = objFunction.CryptographyMD5(model.Password);
                _context.Entry(model).State = EntityState.Modified;
            }else {
                _context.Entry(model).State = EntityState.Modified;
                _context.Entry(model).Property(x => x.Password).IsModified = false;
            }
            await _context.SaveChangesAsync();
            model.Password = "";
            return Ok(new { succeeded = true, message = "Cập nhật thành công", data = model, statusCode = 200 });
        }
    
    }
}
