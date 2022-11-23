using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XHTDHP_API.Models
{
    public class Account
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string FullName { get; set; }
        public string TypeId { get; set; }
        public string State { get; set; }
        public string CodeStore { get; set; }
        public string CodeStoreParent { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string WSTK { get; set; }
    }

}
