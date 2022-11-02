using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XHTDHP_API.Models.Wrapper
{
    public class PagedResponse<T> : Response<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public int TotalRecord { get; set; }

        public PagedResponse(T data, int currentPage, int pageSize)
        {
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
            this.Data = data;
            this.Message = null;
            this.Succeeded = true;
            this.Errors = null;
        }
    }
}