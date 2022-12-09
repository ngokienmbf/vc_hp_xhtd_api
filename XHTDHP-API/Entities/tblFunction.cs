using System;

namespace XHTDHP_API.Entities
{
    public class tblFunction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ItemIndex { get; set; }
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public int GroupIndex { get; set; }
        public DateTime? CreateDay { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDay { get; set; }
        public string UpdateBy { get; set; }
    }
}