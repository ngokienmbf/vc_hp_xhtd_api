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
            var query = _context.tblVehicle.OrderBy(item => item.IDVehicle).AsNoTracking();
            if (!String.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(item => item.Vehicle.Contains(filter.Keyword));
            }
            var totalRecords = await query.CountAsync();
            query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            var pagedData = await query.ToListAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<tblVehicle>(pagedData, filter, totalRecords);
            return Ok(pagedReponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var found = await _context.tblVehicle.Where(item => item.IDVehicle == id).FirstOrDefaultAsync();
            if (found == null)
            {
                return BadRequest("Không tìm thấy phương tiện");
            }
            return Ok(found);
        }

        [HttpPost]
        public async Task<IActionResult> Inser([FromBody] tblVehicle model)
        {
            model.CreateDay = DateTime.Now;
            _context.tblVehicle.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new { succeeded = true, message = "Thêm thành công" });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] tblVehicle model)
        {
            model.UpdateDay = DateTime.Now;
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { succeeded = true, message = "Cập nhật thành công", data = model });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var found = _context.tblVehicle.Find(id);
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

        [HttpGet]
        [Route("getVehiclesNoRfid")]
        public async Task<IActionResult> GetVehiclesNoRfid([FromQuery] PaginationFilter filter)
        {
            var query = from v in _context.tblVehicle
                        join r in _context.tblRFID on v.Vehicle equals r.Vehicle into verf
                        from t in verf.DefaultIfEmpty() where t.Code == null
                        select new tblVehicle
                        {
                            IDVehicle = v.IDVehicle,
                            Vehicle = v.Vehicle,
                            Tonnage = v.Tonnage,
                            TonnageDefault = v.TonnageDefault,
                            NameDriver = v.NameDriver,
                            IdCardNumber = v.IdCardNumber,
                            HeightVehicle = v.HeightVehicle,
                            WidthVehicle = v.WidthVehicle,
                            LongVehicle = v.LongVehicle,
                            UnladenWeight1 = v.UnladenWeight1,
                            UnladenWeight2 = v.UnladenWeight2,
                            UnladenWeight3 = v.UnladenWeight3,
                            IsSetMediumUnladenWeight = v.IsSetMediumUnladenWeight,
                            CreateDay = v.CreateDay,
                            CreateBy = v.CreateBy,
                            UpdateDay = v.UpdateDay,
                            UpdateBy = v.UpdateBy,
                        };
            if (!String.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(item => item.Vehicle.Contains(filter.Keyword));
            }
            var totalRecords = await query.CountAsync();
            query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            var pagedData = await query.ToListAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<tblVehicle>(pagedData, filter, totalRecords);
            return Ok(pagedReponse);
        }
    }
}