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
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public VehicleController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/driver
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var query = _context.Vehicles.OrderBy(item => item.Id).AsNoTracking();
            if (!String.IsNullOrEmpty(filter.SeachKey))
            {
                query = query.Where(item => item.LicensePlace.Contains(filter.SeachKey));
            }
            var totalRecords = await query.CountAsync();
            query = query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);
            var pagedData = await query.ToListAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Vehicle>(pagedData, filter, totalRecords);
            return Ok(pagedReponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var found = await _context.Vehicles.FindAsync(id);
            return Ok(new { succeeded = true, message = "Lấy dữ liệu thành công", data = found });
        }

        [HttpPost]
        public async Task<IActionResult> Inser([FromBody] Vehicle model)
        {
            model.CreatedOn = DateTime.Now;
            _context.Vehicles.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new { succeeded = true, message = "Thêm thành công" });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Vehicle model)
        {
            model.ModifiedOn = DateTime.Now;
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { succeeded = true, message = "Cập nhật thành công", data = model });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var found = _context.Vehicles.Find(id);
            if (found != null)
            {
                _context.Entry(found).State = EntityState.Deleted;
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