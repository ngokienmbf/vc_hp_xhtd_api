using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace XHTDHP_API.Entities
{
    public class tblCategoriesDevices
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Id { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]         
        public string Code { get; set; }
        public string? Name { get; set; }
        public string? CatCode { get; set; }
        public string? ManCode { get; set; }
        public string? IpAddress { get; set; }
        public Nullable<int> PortNumber { get; set; }
        public Nullable<int> PortNumberDeviceIn { get; set; }
        public Nullable<int> PortNumberDeviceOut { get; set; }
        public Nullable<int> PortNumberDeviceIn1 { get; set; }
        public Nullable<int> PortNumberDeviceOut1 { get; set; }
        public Nullable<int> PortNumberDeviceIn2 { get; set; }
        public Nullable<int> PortNumberDeviceOut2 { get; set; }
        public string? Descriptioon { get; set; }
        public bool State { get; set; } = true;
        public Nullable<int> ShowIndex { get; set; }
        public DateTime? CreateDay { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDay { get; set; }
        public string UpdateBy { get; set; }
    }
}