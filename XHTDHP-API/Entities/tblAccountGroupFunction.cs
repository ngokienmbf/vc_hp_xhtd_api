using System;

namespace XHTDHP_API.Entities
{
    public class tblAccountGroupFunction
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int FunctionId { get; set; }
        public bool F_Add { get; set; } = true;
        public bool F_Edit { get; set; } = true;
        public bool F_Del { get; set; } = true;
        public bool F_View { get; set; } = true;
        public bool F_Print { get; set; } = true;
        public bool F_Other { get; set; } = true;
        public DateTime? CreateDay { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDay { get; set; }
        public string UpdateBy { get; set; }
    }
}