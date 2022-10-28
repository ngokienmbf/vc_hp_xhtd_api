namespace XHTDHP_API.Models.Filter
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SeachKey { get; set; }
        public PaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
            this.SeachKey = "";
        }
        public PaginationFilter(int pageNumber, int pageSize, string SeachKey)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
            this.SeachKey = SeachKey;
        }
    }
}
