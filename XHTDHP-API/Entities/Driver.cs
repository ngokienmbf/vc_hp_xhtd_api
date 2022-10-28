using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XHTDHP_API.Entities
{
    public class Driver : BaseClass
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(250)]
        public string FullName { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string Phone { get; set; }
        public DateTime Birthday { get; set; }
        [MaxLength(50)]
        public string Gender { get; set; }
        [MaxLength(50)]
        public string IdCard { get; set; }
        [MaxLength(500)]
        public string Address { get; set; }
        [MaxLength(50)]
        public string UserName { get; set; }
        public bool State { get; set; }
    }
}