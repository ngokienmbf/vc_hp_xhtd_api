using System;

namespace XHTDHP_API.Entities
{
    public class tblAccountGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool State { get; set; } = true;
        public DateTime? CreateDay { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDay { get; set; }
        public string UpdateBy { get; set; }
    }
}