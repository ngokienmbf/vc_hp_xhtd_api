using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XHTDHP_API.Entities
{
    public class RFID : BaseClass
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Vehicle { get; set; }
        public DateTime DayReleased { get; set; }
        public DateTime DayExpired { get; set; }
        public string Note { get; set; }
        public bool State { get; set; }
        public DateTime LastEnter { get; set; }
    }
}