using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace XHTDHP_API.Entities
{
    public class tblDriver
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string UserName { get; set; }
        public bool State { get; set; } = true;
        [MaxLength(250)]
        public string FullName { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string Phone { get; set; }
        public DateTime? Birthday { get; set; }
        [MaxLength(50)]
        public string Gender { get; set; }
        [MaxLength(50)]
        public string IdCard { get; set; }
        [MaxLength(500)]
        public string Address { get; set; }
        public DateTime? CreateDay { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDay { get; set; }
        public string UpdateBy { get; set; }
        [NotMapped]
        public List<String> Vehicles {get; set;}
    }
}