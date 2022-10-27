using System.ComponentModel.DataAnnotations;

namespace xhtdApi.Models
{
    public class SumProfileResponseDTO
    {
        public string access_token { get; set; }
        public double expires_in { get; set; }
        public string token_type { get; set; }
    }
    public class LoginDto
    {
        public string grant_type { get; set; } = "password";
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
}
