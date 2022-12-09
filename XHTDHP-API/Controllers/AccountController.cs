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
using XHTDHP_API.Services;
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
using XHTDHP_API.Utils;

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
        IEmailSender _emailSender = new AuthMessageSender();
  
        //public AccountController(IConfiguration configuration, ILoggerManager logger)
        //{
        //    _configuration = configuration;
        //    _logger = logger;
        //    objFunction = new Function(configuration);
        //}

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("login")]
        public async Task<ActionResult<Object>> Login([FromBody] LoginDto model)
        {
            var responseModel = new SumProfileResponseDTO();
            responseModel.ListRole = new List<string>();
            
            if (String.IsNullOrEmpty(model.userName) || String.IsNullOrEmpty(model.password))
            {
                responseModel.Message = "Không được để trống tài khoản và mật khẩu";
                return responseModel;
            }
            else
            {
                var checkUserNameAndPass = objFunction.checkUserNameAndPassWord(model.userName, model.password);
                
                if (!checkUserNameAndPass) 
                {
                    responseModel.Message = "Sai tài khoản hoặc mật khẩu";
                    return responseModel;
                }
                
                var account = _context.tblAccount.FirstOrDefault(u => u.UserName == model.userName);
                if (account != null)
                {
                    var user = new AppUser
                    {
                        UserName = account.UserName,
                        NormalizedUserName = account.UserName,
                        PasswordHash = account.Password,
                    };

                    // var lstRole = await _userManager.GetRolesAsync(user);
                    // responseModel.ListRole = _mapper.Map<List<string>>(lstRole);
                    
                    //responseModel.ListRole.Add("Admin");
                   
                    responseModel.ListRole.Add(account.GroupId.ToString());
                    responseModel.GroupId = account.GroupId;
                    responseModel.GroupFunctions = await _context.tblAccountGroupFunction.Where(item => item.GroupId == account.GroupId)
                    .ToListAsync();

                    
                    var jwt = await GenerateJwtToken(user);
                    var expireIn = Convert.ToDouble(3600);
                    responseModel.expires_in = expireIn;
                    responseModel.access_token = jwt.ToString();
                    responseModel.token_type = "bearer";
                    responseModel.ErrorCode = "200";
                    return responseModel;
                } else {
                    return responseModel;
                }
            }
        }

        [HttpPost("register")]
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

        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] int id)
        {
            var found = await _context.tblAccount.Where(item => item.Id == id).FirstOrDefaultAsync();
            if (found != null)
            {
                found.Password = objFunction.CryptographyMD5(found.Password);
                _context.Entry(found).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(new { succeeded = true, message = "Khôi phục mật khẩu mặc định thành công" });
            }
            else
            {
                return BadRequest(new { succeeded = false, message = "Có lỗi xảy ra" });
            }
        }


        // --- forgot, verify, set new password

        //[HttpPost("ForgotPassword")]
        private async Task<ActionResult<Object>> ForgotPassword([FromBody] ForgotPassworDTO model)
        {
            var responseModel = new ForgotPassworResponseDTO();
            responseModel.EmailOrPhone = model.EmailOrPhone;

            //var user = await _context.FindByEmailAsync(model.EmailOrPhone);
            var user = _context.tblAccount.FirstOrDefault(u => u.Email.ToLower() == model.EmailOrPhone.ToLower());
            if (user == null)
            {
                responseModel.ErrorCode = "ACC008";
                responseModel.Message = ConstMessage.GetMsgConst("ACC008");
                return responseModel;
            }
            var codeMail = objFunction.CryptographyMD5(model.EmailOrPhone.ToLower()+DateTime.Now.ToShortDateString().ToString());
            await SendCode("Email", model.EmailOrPhone, codeMail);
            responseModel.ErrorCode = "00";
            responseModel.Message = "Đã gửi code xác nhận qua email";
            return responseModel;
        }

        /// <remarks>
        /// Xác thực mã code sau khi nhận được từ email
        /// </remarks>
        /// <returns></returns>
        //[HttpPost("VerifyCode")]
        private async Task<ActionResult<Object>> VerifyCode(VerifyCodeDTO model)
        {
            var responseModel = new ConfirmVerify();
            responseModel.EmailOrPhone = model.email;
            responseModel.Code = model.code;
            var user = _context.tblAccount.FirstOrDefault(u => u.Email.ToLower() == model.email.ToLower());
            if (user == null)
            {
                responseModel.ErrorCode = "ACC008";
                responseModel.Message = ConstMessage.GetMsgConst("ACC008");
                return responseModel;
            }

            var codeMail = objFunction.CryptographyMD5(model.email.ToLower()+DateTime.Now.ToShortDateString().ToString());
            var result =  model.code == codeMail;
            if (result)
            {
                responseModel.Code = "00";
                responseModel.Message = "Verify Thành công";
                return responseModel;
            }
            else
            {
                responseModel.ErrorCode = "ACC012";
                responseModel.Message = "Verify không thành công";
                return responseModel;
            }
        }


        /// <remarks>
        /// Hàm gửi lại mã code vào email, dành cho trường hợp cần gửi lại
        /// </remarks>
        /// <returns></returns>
        //[HttpPost("ResendCode")]
        private async Task<ActionResult<Object>> ResendCode([FromBody] ForgotPassworDTO model)
        {
            var responseModel = new ModelBase();
            var code = "";
            var user = _context.tblAccount.FirstOrDefault(u => u.Email.ToLower() == model.EmailOrPhone.ToLower());

            if (Util.IsPhoneNumber(model.EmailOrPhone))
            {
                responseModel.ErrorCode = "01";
                responseModel.Message = "Hệ thống chưa hỗ trợ số điện thoại, vui lòng sử dụng email để thử lại";
                return responseModel;
            }
                if (user == null)
            {
                responseModel.ErrorCode = "ACC008";
                responseModel.Message = ConstMessage.GetMsgConst("ACC008");
                return responseModel;
            }
            if (!Util.IsPhoneNumber(model.EmailOrPhone))
            {
                code = objFunction.CryptographyMD5(model.EmailOrPhone.ToLower()+DateTime.Now.ToShortDateString().ToString());
            }
            else
            {
                code = objFunction.CryptographyMD5(model.EmailOrPhone.ToLower()+DateTime.Now.ToShortDateString().ToString());
            }


            await SendCode(Util.IsPhoneNumber(model.EmailOrPhone) ? "Phone" : "Email", model.EmailOrPhone, code);
            responseModel.ErrorCode = "00";
            responseModel.Message = "Đã gửi code xác nhận";
            return responseModel;
        }

        private async Task<ActionResult<Object>> SendCode(string provider, string emailOrPhone, string code)
        {
            await _emailSender.SendEmailAsync(emailOrPhone, "Mã xác thực lấy lại mật khẩu", $"Mã xác thực của bạn là:{code}", new EmailConfig());
            //if (provider == "Email")
            //{
            //    await _emailSender.SendEmailAsync(emailOrPhone, "Mã xác thực lấy lại mật khẩu", $"Mã xác thực của bạn là:{code}", _repositoryWrapper.AspNetUsers.setting());
            //}
            //else if (provider == "Phone")
            //{
            //    Util.SendSMS($"Ma code xac thuc cua ban la: {code}", emailOrPhone);
            //}

            return NoContent();
        }
    
    }
    
}
