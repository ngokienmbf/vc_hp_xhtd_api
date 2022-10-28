using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XHTDHP_API.Entities;

namespace XHTDHP_API.Models
{
    public class DriverDTO : BaseClass
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class DriverCreateDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}