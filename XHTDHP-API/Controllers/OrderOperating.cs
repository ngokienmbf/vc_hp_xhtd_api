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
    public class OrderOperating : ControllerBase
    {
        private readonly ApiDbContext _context;

        public OrderOperating(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/driver
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var query = _context.tblStoreOrderOperating.AsNoTracking();
            if (!String.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(item => item.Vehicle.Contains(filter.Keyword));
            }
            if (!String.IsNullOrEmpty(filter.DeliveryCode))
            {
                query = query.Where(item => item.DeliveryCode.Contains(filter.DeliveryCode));
            }
            if (!String.IsNullOrEmpty(filter.State))
            {
                query = query.Where(item => item.State.ToLower() == filter.State.ToLower());
            }

            var totalRecords = await query.CountAsync();
            query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            var pagedData = await query.ToListAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<tblStoreOrderOperating>(pagedData, filter, totalRecords);
            return Ok(pagedReponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var found = await _context.tblStoreOrderOperating.FindAsync(id);
            return Ok(new { succeeded = true, message = "Lấy dữ liệu thành công", data = found, statusCode = 200 });
        }

        // [HttpPost]
        // public async Task<IActionResult> Inser([FromBody] tblStoreOrderOperating model)
        // {
        //     model.CreateDay = DateTime.Now;
        //     _context.tblStoreOrderOperating.Add(model);
        //     await _context.SaveChangesAsync();
        //     return Ok(new { succeeded = true, message = "Thêm thành công" });
        // }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] tblStoreOrderOperating model)
        {
            model.UpdateDay = DateTime.Now;
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { succeeded = true, message = "Cập nhật thành công", data = model, statusCode = 200 });
        }

        // [HttpDelete("{id}")]
        // public IActionResult Delete(int id)
        // {
        //     var found = _context.tblStoreOrderOperating.Find(id);
        //     if (found != null)
        //     {
        //         _context.Entry(found).State = EntityState.Deleted;
        //         _context.SaveChanges();
        //         return Ok(new { succeeded = true, message = "Xoá thành công" });
        //     }
        //     else
        //     {
        //         return BadRequest(new { succeeded = false, message = "Có lỗi xảy ra" });
        //     }
        // }
    }
}