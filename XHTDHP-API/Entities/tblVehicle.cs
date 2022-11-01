using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XHTDHP_API.Entities
{
    public class tblVehicle
    {
        [Key]
        public int IDVehicle { get; set; }
        public int IDStore { get; set; }
        public string Vehicle { get; set; }
        public double Tonnage { get; set; }
        public double TonnageDefault { get; set; }
        public string NameDriver { get; set; }
        public string IdCardNumber { get; set; }
        public string HeightVehicle { get; set; }
        public string WidthVehicle { get; set; }
        public string LongVehicle { get; set; }
        public DateTime? DayCreate { get; set; }
        public DateTime? DayUpdate { get; set; }
        public string UserCreate { get; set; }
        public string UserUpdate { get; set; }
        public int UnladenWeight1 { get; set; }
        public int UnladenWeight2 { get; set; }
        public int UnladenWeight3 { get; set; }
        public bool IsSetMediumUnladenWeight { get; set; }
        public DateTime? CreateDay { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDay { get; set; }
        public string UpdateBy { get; set; }
    }
}