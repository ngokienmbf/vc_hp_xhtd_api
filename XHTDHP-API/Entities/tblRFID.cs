using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XHTDHP_API.Entities
{
    public class tblRFID
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Vehicle { get; set; }
        public Nullable<System.DateTime> DayReleased { get; set; }
        public Nullable<System.DateTime> DayExpired { get; set; }
        public string Note { get; set; }
        public Nullable<bool> State { get; set; }
        public Nullable<System.DateTime> Createday { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateDay { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> LastEnter { get; set; }
    }
}