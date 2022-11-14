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
    public class RFIDController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public RFIDController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/driver
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var query = _context.tblRFID.OrderBy(item => item.Id).AsNoTracking();
            if (!String.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(item => item.Code.Contains(filter.Keyword) || item.Vehicle.Contains(filter.Keyword));
            }
            var totalRecords = await query.CountAsync();
            query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            var pagedData = await query.ToListAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<tblRFID>(pagedData, filter, totalRecords);
            return Ok(pagedReponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var found = await _context.tblRFID.FindAsync(id);
            return Ok(found);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] tblRFID model)
        {
            model.Createday = DateTime.Now;
            _context.tblRFID.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new { succeeded = true, message = "Thêm thành công", statusCode = 200 });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] tblRFID model)
        {
            model.UpdateDay = DateTime.Now;
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { succeeded = true, message = "Cập nhật thành công", data = model, statusCode = 200 });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var found = _context.tblRFID.Find(id);
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