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
            var query = _context.tblVehicle.OrderBy(item => item.Vehicle).AsNoTracking();
            if (!String.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(item => item.Vehicle.Contains(filter.Keyword));
            }
            var totalRecords = await query.CountAsync();
            query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            var pagedData = await query.ToListAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<tblVehicle>(pagedData, filter, totalRecords);
            
            // lay driver duoc gan vao
            var driverVehicles = await _context.tblDriverVehicle.OrderBy(item => item.Vehicle).Where(item => item.Vehicle.Contains(filter.Keyword)).ToListAsync();
            int tmp = 0;
            for (int i = 0; i < driverVehicles.Count(); i++)
            {
                for (int j = tmp; j < pagedData.Count(); j++)
                {
                   
                    if (driverVehicles[i].Vehicle == pagedData[j].Vehicle)
                    {
                        if(pagedData[j].Drivers == null){ 
                            pagedData[j].Drivers = new List<string>();
                        }
                        pagedData[j].Drivers.Add(driverVehicles[i].UserName);
                        tmp = j; // tang hieu suat
                        break;
                    }
                }
            }
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
        
        [HttpGet("GetFull")]
        public IActionResult GetFull(string vehicle)
        {
            // var found =  _context.tblVehicle.Where( item => item.Vehicle ==  vehicle || 
            //            !_context.tblDriverVehicle.Any(f => f.Vehicle == item.Vehicle))
            //             .Select(item => new {Id = item.IDVehicle, Vehicle = item.Vehicle}).ToList();
            var query =  _context.tblVehicle.OrderBy(item => item.Vehicle).ToList();
            return Ok(query);
        }
        
        [HttpGet("GetWithDriver/{id}")]
        public async Task<IActionResult> GetWithVehicle(int id)
        {
            // 1 - 1
            // var found = await (from p in _context.tblVehicle join o in _context.tblDriverVehicle
            // on p.Vehicle equals o.Vehicle into gj from subpet in gj.DefaultIfEmpty()
            // where p.IDVehicle == id
            // select new  
            // { 
            //      p.IDVehicle
            //     ,p.Vehicle
            //     ,p.Tonnage
            //     ,p.TonnageDefault
            //     ,p.NameDriver
            //     ,p.IdCardNumber
            //     ,p.HeightVehicle
            //     ,p.WidthVehicle
            //     ,p.LongVehicle
            //     ,p.UnladenWeight1
            //     ,p.UnladenWeight2
            //     ,p.UnladenWeight3
            //     ,p.IsSetMediumUnladenWeight
            //     ,p.CreateDay
            //     ,p.CreateBy
            //     ,p.UpdateDay
            //     ,p.UpdateBy,
            //      UserName = subpet.UserName ?? string.Empty
            // }).FirstOrDefaultAsync();     

            var vehicle = await _context.tblVehicle.FindAsync(id);
            var found = await _context.tblDriverVehicle.Where(item => item.Vehicle==vehicle.Vehicle)
                .Select(item => item.UserName).ToListAsync();
            vehicle.Drivers = found;
            return Ok(vehicle);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] tblVehicle model)
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
        public async Task<IActionResult> Delete(int id)
        {
            var found1 = _context.tblVehicle.Find(id);
            if (found1 != null)
            {
                _context.Entry(found1).State = EntityState.Deleted;
                //xoa bang trung gian
                var found = await _context.tblDriverVehicle.Where(item => item.Vehicle == found1.Vehicle).ToListAsync();
                
                for (int i = 0; i < found.Count(); i++)
                {
                    _context.Entry(found[i]).State = EntityState.Deleted;
                }
                await _context.SaveChangesAsync();
                return Ok(new { succeeded = true, message = "Xoá thành công", statusCode = 200 });
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

        [HttpGet]
        [Route("license-plate/{value}")]
        public async Task<IActionResult> GetByLP(string value)
        {
            var found = await _context.tblVehicle.Where(item => item.Vehicle == value).FirstOrDefaultAsync();
            if (found == null)
            {
                return BadRequest("Không tìm thấy phương tiện");
            }
            return Ok(found);
        }
    }
}