using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XHTDHP_API.Data;
using XHTDHP_API.Entities;
using XHTDHP_API.Helpers;
using XHTDHP_API.Models;
using XHTDHP_API.Models.Filter;

namespace XHTDHP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountGroupFunctionController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public AccountGroupFunctionController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/driver
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var query = _context.tblAccountGroupFunction.OrderBy(item => item.GroupId).AsNoTracking();
            // if (!String.IsNullOrEmpty(filter.Keyword))
            // {
            //     query = query.Where(item => item.GroupId.Contains(filter.Keyword));
            // }
            var totalRecords = await query.CountAsync();
            query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            var pagedData = await query.ToListAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<tblAccountGroupFunction>(pagedData, filter, totalRecords);
            return Ok(pagedReponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var found = await _context.tblAccountGroupFunction.FindAsync(id);
            return Ok(found);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] tblAccountGroupFunction model)
        {
            model.CreateDay = DateTime.Now;
            _context.tblAccountGroupFunction.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new { succeeded = true, message = "Thêm thành công", statusCode = 200 });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] tblAccountGroupFunction model)
        {
            model.UpdateDay = DateTime.Now;
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { succeeded = true, message = "Cập nhật thành công", data = model, statusCode = 200 });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var found = _context.tblAccountGroupFunction.Find(id);
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

        [HttpGet("GetFullFunctions")]
        public async Task<IActionResult> GetFullFunctions()
        {
            var query = await _context.tblFunction.OrderBy(item => item.ItemIndex).ToListAsync();
            return Ok(query);
        }

    }
}