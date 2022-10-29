using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using XHTDHP_API.Data;
using XHTDHP_API.Entities;
using XHTDHP_API.Helpers;
using XHTDHP_API.Models;
using XHTDHP_API.Models.Filter;

namespace XHTDHP_API.Controllers
{
    [Route("api/[controller]")]
    public class DriverController : ControllerBase
    {
        private readonly ILogger<DriverController> _logger;
        private readonly ApiDbContext _context;

        public DriverController(ILogger<DriverController> logger, ApiDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: api/driver
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var query = _context.Drivers.OrderBy(item => item.Id).AsNoTracking();
            if (!String.IsNullOrEmpty(filter.SeachKey))
            {
                query = query.Where(item => item.FullName.Contains(filter.SeachKey));
            }
            var totalRecords = await query.CountAsync();
            query = query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);
            var pagedData = await query.ToListAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Driver>(pagedData, filter, totalRecords);
            return Ok(pagedReponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            return Ok(new { succeeded = true, message = "Lấy dữ liệu thành công", data = driver });
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] Driver model)
        {
            model.CreatedOn = DateTime.Now;
            _context.Drivers.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new { succeeded = true, message = "Thêm thành công" });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Driver model)
        {
            model.ModifiedOn = DateTime.Now;
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { succeeded = true, message = "Cập nhật thành công", data = model });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var driver = _context.Drivers.Find(id);
            if (driver != null)
            {
                _context.Entry(driver).State = EntityState.Deleted;
                _context.SaveChanges();
                return Ok(new { succeeded = true, message = "Xoá thành công" });
            }
            else
            {
                return BadRequest(new { succeeded = false, message = "Có lỗi xảy ra" });
            }
        }
    }
}