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
        public string Vehicle { get; set; }
        public Nullable<double> Tonnage { get; set; }
        public Nullable<double> TonnageDefault { get; set; }
        public string NameDriver { get; set; }
        public string IdCardNumber { get; set; }
        public double HeightVehicle { get; set; }
        public double WidthVehicle { get; set; }
        public double LongVehicle { get; set; }
        public Nullable<int> UnladenWeight1 { get; set; }
        public Nullable<int> UnladenWeight2 { get; set; }
        public Nullable<int> UnladenWeight3 { get; set; }
        public Nullable<bool> IsSetMediumUnladenWeight { get; set; }
        public Nullable<System.DateTime> CreateDay { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateDay { get; set; }
        public string UpdateBy { get; set; }
    }
}