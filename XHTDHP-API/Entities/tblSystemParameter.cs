using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XHTDHP_API.Entities
{
    public class tblSystemParameter
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string? Name { get; set; }
        public string? Value { get; set; }
        public DateTime CreateDay { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDay { get; set; }
        public string? UpdateBy { get; set; }
    }
}