using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XHTDHP_API.Entities
{
    public class tblRFIDSign
    {
        [Key]
        public int Id { get; set; }
        public string Vehicle { get; set; }
        public string RfidCode { get; set; }
        public string Image { get; set; }
        public Nullable<System.DateTime> CreateDay { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateDay { get; set; }
        public string UpdateBy { get; set; }
    }
}