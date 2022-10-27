using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xhtdApi.Entities
{
    public class BaseClass
    {
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = "";
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; } = "";
    }
}