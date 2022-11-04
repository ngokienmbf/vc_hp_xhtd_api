using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XHTDHP_API.Entities
{
    public class tblTrough
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(5)]
        public string Code { get; set; }
        public string Name { get; set; }
        public Nullable<double> Height { get; set; }
        public Nullable<double> Width { get; set; }
        public Nullable<double> Long { get; set; }
        public Nullable<bool> Working { get; set; }
        public Nullable<bool> Problem { get; set; }
        public Nullable<bool> State { get; set; }
        public string DeliveryCodeCurrent { get; set; }
        public Nullable<double> PlanQuantityCurrent { get; set; }
        public Nullable<double> CountQuantityCurrent { get; set; }
        public Nullable<bool> IsPicking { get; set; }
        public string TransportNameCurrent { get; set; }
        public Nullable<System.DateTime> CheckInTimeCurrent { get; set; }
        public Nullable<bool> IsInviting { get; set; }
        [MaxLength(2)]
        public string LineCode { get; set; }
        public Nullable<System.DateTime> CreateDay { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateDay { get; set; }
        public string UpdateBy { get; set; }
    }
}