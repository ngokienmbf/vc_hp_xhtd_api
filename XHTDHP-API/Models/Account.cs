using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace XHTDHP_API.Models
{
    public class Account
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string FullName { get; set; }
        public string TypeId { get; set; }
        public string State { get; set; }
        public string CodeStore { get; set; }
        public string CodeStoreParent { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string WSTK { get; set; }
    }

    public class VerifyCodeDTO
    {
        public string email { get; set; }
        public string code { get; set; }
    }
    public class LoginTokenDTO
    {
        public string Email { get; set; }
        public string Jwt { get; set; }
    }

    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
        public string Password { get; set; }
    }

    public class ChangePassDTO : ModelBase
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
        public string OldPassword { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
        public string NewPassword { get; set; }
    }

    public class ForgotPassworDTO
    {
        public string EmailOrPhone { get; set; }
    }
    public class ForgotPassworResponseDTO : ModelBase
    {
        public string EmailOrPhone { get; set; }
        public string Code { get; set; }
    }
    public class ConfirmVerify : ModelBase
    {
        public string EmailOrPhone { get; set; }
        public string Code { get; set; }
        public List<string> ListRole { get; set; }
        public string JWT { get; set; }
        public double ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }

}
