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
    public class DriverVehicleController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public DriverVehicleController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/driver
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var query = _context.tblDriverVehicle.OrderBy(item => item.Id).AsNoTracking();
            if (!String.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(item => item.Vehicle.Contains(filter.Keyword) || item.UserName.Contains(filter.Keyword) );
            }
            var totalRecords = await query.CountAsync();
            query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            var pagedData = await query.ToListAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<tblDriverVehicle>(pagedData, filter, totalRecords);
            return Ok(pagedReponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var found = await _context.tblDriverVehicle.FindAsync(id);
            if (found == null)
            {
                return BadRequest("Không tìm thấy phương tiện");
            }
            return Ok(found);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] tblDriverVehicle model)
        {
            var found = await _context.tblDriverVehicle
            .Where(item => item.Vehicle == model.Vehicle && item.UserName == model.UserName )
            .FirstOrDefaultAsync();
            if(found != null){
                return Ok(new { succeeded = false, message = "Đã tồn tại", data = model, statusCode = 200 });
            }

            model.CreateDay = DateTime.Now;
            _context.tblDriverVehicle.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new { succeeded = true, message = "Thêm thành công" });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] tblDriverVehicle model)
        {
            var found = await _context.tblDriverVehicle
            .Where(item => item.Vehicle == model.Vehicle && item.UserName == model.UserName && item.Id != model.Id)
            .FirstOrDefaultAsync();
            
            if(found != null){
                return Ok(new { succeeded = false, message = "Đã tồn tại", data = model, statusCode = 200 });
            } else {
                model.UpdateDay = DateTime.Now;
                _context.Entry(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(new { succeeded = true, message = "Cập nhật thành công", data = model, statusCode = 200 });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var found = _context.tblDriverVehicle.Find(id);
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