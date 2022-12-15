using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using XHTDHP_API.Entities;

namespace XHTDHP_API.Models
{
    public class SumProfileResponseDTO : ModelBase
    {
        public string access_token { get; set; }
        public double expires_in { get; set; }
        public string token_type { get; set; }
        public List<string> ListRole { get; set; }
        public int GroupId { get; set; }
        public List<tblAccountGroupFunction> GroupFunctions { get; set; }
    }
    public class LoginDto
    {
        public string grant_type { get; set; } = "password";
        [Required]
        public string userName { get; set; }
        [Required]
        public string password { get; set; }
    }
}
