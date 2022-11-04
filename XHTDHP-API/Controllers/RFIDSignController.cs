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
    public class RFIDSignController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public RFIDSignController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/driver
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var query = _context.tblRFIDSign.OrderBy(item => item.Id).AsNoTracking();
            if (!String.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(item => item.RfidCode.Contains(filter.Keyword) || item.Vehicle.Contains(filter.Keyword));
            }
            var totalRecords = await query.CountAsync();
            query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            var pagedData = await query.ToListAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<tblRFIDSign>(pagedData, filter, totalRecords);
            return Ok(pagedReponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var found = await _context.tblRFIDSign.FindAsync(id);
            return Ok(found);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] tblRFIDSign model)
        {
            model.CreateDay = DateTime.Now;
            var rfidExist = await _context.tblRFID.Where(item => item.Code == model.RfidCode).FirstOrDefaultAsync();
            if (rfidExist == null)
            {
                return BadRequest("rfid không tồn tại");
            }
            var found = await _context.tblRFIDSign.Where(item => item.RfidCode == model.RfidCode).FirstOrDefaultAsync();
            rfidExist.Vehicle = model.Vehicle;
            _context.tblRFID.Update(rfidExist);
            await _context.SaveChangesAsync();
            if (found == null)
            {
                _context.tblRFIDSign.Add(model);
                await _context.SaveChangesAsync();
                return Ok(new { succeeded = true, message = "Thêm thành công", statusCode = 200 });
            } 
            else
            {
                found.Vehicle = model.Vehicle;
                found.UpdateDay = DateTime.Now;
                _context.tblRFIDSign.Update(found);
                await _context.SaveChangesAsync();
                return Ok(new { succeeded = true, message = "Cập nhật thành công", statusCode = 200 });
            } 
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] tblRFIDSign model)
        {
            model.UpdateDay = DateTime.Now;
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { succeeded = true, message = "Cập nhật thành công", data = model, statusCode = 200 });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var found = _context.tblRFIDSign.Find(id);
            if (found != null)
            {
                _context.Entry(found).State = EntityState.Deleted;
                _context.SaveChanges();
                return Ok(new { succeeded = true, message = "Xoá thành công", statusCode = 200 });
            }
            else
            {
                return BadRequest(new { succeeded = false, message = "Có lỗi xảy ra", statusCode = 500 });
            }
        }
    }
}