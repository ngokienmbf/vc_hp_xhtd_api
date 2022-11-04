using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XHTDHP_API.Data;
using XHTDHP_API.Entities;
using XHTDHP_API.Helpers;
using XHTDHP_API.Models.Filter;

namespace XHTDHP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TroughTypeProductController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public TroughTypeProductController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/driver
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var query = _context.tblTroughTypeProduct.OrderBy(item => item.Id).AsNoTracking();
            if (!String.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(item => item.TroughCode.Contains(filter.Keyword));
            }
            var totalRecords = await query.CountAsync();
            query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            var pagedData = await query.ToListAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<tblTroughTypeProduct>(pagedData, filter, totalRecords);
            return Ok(pagedReponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var driver = await _context.tblTroughTypeProduct.FindAsync(id);
            return Ok(driver);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] tblTroughTypeProduct model)
        {
            _context.tblTroughTypeProduct.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new { succeeded = true, message = "Thêm thành công", statusCode = 200 });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] tblTroughTypeProduct model)
        {
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { succeeded = true, message = "Cập nhật thành công", data = model, statusCode = 200 });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var found = _context.tblTroughTypeProduct.Find(id);
            if (found != null)
            {
                _context.Entry(found).State = EntityState.Deleted;
                _context.SaveChanges();
                return Ok(new { succeeded = true, message = "Xoá thành công", statusCode = 200 });
            }
            else
            {
                return BadRequest(new { succeeded = false, message = "Có lỗi xảy ra" });
            }
        }
    }
}