namespace XHTDHP_API.Models.Filter
{
    public class PaginationFilter
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }
        public string State { get; set; } = "";
        public string DeliveryCode { get; set; }
        public PaginationFilter()
        {
            this.Page = 1;
            this.PageSize = 10;
            this.Keyword = "";
        }
        public PaginationFilter(int page, int pageSize, string keyword)
        {
            this.Page = page < 1 ? 1 : page;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
            this.Keyword = keyword;
        }
        public PaginationFilter(int page, int pageSize, string Keyword, string state)
        {
            this.Page = page < 1 ? 1 : page;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
            this.Keyword = Keyword;
            this.State = state;
        }
        public PaginationFilter(int page, int pageSize, string Keyword, string state, string deliveryCode)
        {
            this.Page = page < 1 ? 1 : page;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
            this.Keyword = Keyword;
            this.State = state;
            this.DeliveryCode = deliveryCode;
        }
    }
}
