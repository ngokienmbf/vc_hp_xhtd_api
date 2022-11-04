using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XHTDHP_API.Entities
{
    public class tblTroughTypeProduct
    {
        [Key]
        public int Id { get; set; }
        public string TroughCode { get; set; }
        public string TypeProduct { get; set; }
        public Nullable<int> Status { get; set; }
        public string LogResponse { get; set; }
        public Nullable<System.DateTime> CreateDay { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateDay { get; set; }
        public string UpdateBy { get; set; }
    }
}