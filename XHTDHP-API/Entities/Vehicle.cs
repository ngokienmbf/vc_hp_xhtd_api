using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XHTDHP_API.Entities
{
    public class Vehicle : BaseClass
    {
        public int Id { get; set; }
        public string NameDriver { get; set; }
        public string LicensePlace { get; set; }
        public float Tonnage { get; set; }
        public float TonnageDefault { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public string DrivingLicense { get; set; }
    }
}