
using System;
using System.Collections.Generic;
using XHTDHP_API.Models.Filter;
using XHTDHP_API.Models.Wrapper;

namespace XHTDHP_API.Helpers
{
    public class PaginationHelper
    {
        public static PagedResponse<List<T>> CreatePagedReponse<T>(List<T> pagedData, PaginationFilter validFilter, int totalRecords)
        {
            var respose = new PagedResponse<List<T>>(pagedData, validFilter.Page, validFilter.PageSize);
            var totalPages = ((double)totalRecords / (double)validFilter.PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            respose.TotalPage = roundedTotalPages;
            respose.TotalRecord = totalRecords;
            return respose;
        }
    }
}