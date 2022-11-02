using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace XHTDHP_API.Entities
{
    public class tblDevice
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Id { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]         
        public string Code { get; set; }
        public string CodeParent { get; set; }
        public string? Name { get; set; }
        public Nullable<int> OperID { get; set; }
        public Nullable<int> DoorOrAuxoutID { get; set; }
        public Nullable<int> OutputAddrType { get; set; }
        public Nullable<int> DoorAction { get; set; }
        public Nullable<int> InputPort { get; set; }
        public Nullable<int> OutputPort { get; set; }
        public string?  Ipaddress { get; set; }
        public string? Port { get; set; }
        public DateTime? CreateDay { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDay { get; set; }
        public string UpdateBy { get; set; }
    }
}