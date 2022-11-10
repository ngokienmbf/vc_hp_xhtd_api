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
            
            var driverVehicles = await _context.tblDriverVehicle.OrderBy(item => item.Vehicle).Where(item => item.Vehicle.Contains(filter.Keyword)).ToListAsync();
            
            int tmp = 0;
            for (int i = 0; i < driverVehicles.Count(); i++)
            {
                for (int j = tmp; j < pagedData.Count(); j++)
                {
                    if (driverVehicles[i].Vehicle == pagedData[j].Vehicle)
                    {
                        pagedData[j].UserName = driverVehicles[i].UserName;
                        tmp = j+1; // tang hieu suat
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
        
        [HttpGet("GetFreeVehicles/{vehicle}")]
        public IActionResult getFreeVehicles(string vehicle)
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
            var found = await (from p in _context.tblVehicle join o in _context.tblDriverVehicle
            on p.Vehicle equals o.Vehicle into gj from subpet in gj.DefaultIfEmpty()
            where p.IDVehicle == id
            select new  
            { 
                 p.IDVehicle
                ,p.Vehicle
                ,p.Tonnage
                ,p.TonnageDefault
                ,p.NameDriver
                ,p.IdCardNumber
                ,p.HeightVehicle
                ,p.WidthVehicle
                ,p.LongVehicle
                ,p.UnladenWeight1
                ,p.UnladenWeight2
                ,p.UnladenWeight3
                ,p.IsSetMediumUnladenWeight
                ,p.CreateDay
                ,p.CreateBy
                ,p.UpdateDay
                ,p.UpdateBy,
                 UserName = subpet.UserName ?? string.Empty
            }).FirstOrDefaultAsync();     

            return Ok(found);
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
            
            if (!String.IsNullOrEmpty(model.UserName)) {
                var found = await _context.tblDriverVehicle.Where(item => item.Vehicle == model.Vehicle).FirstOrDefaultAsync();
                
                if(found != null && found.UserName != model.UserName ){
                    if(found.UserName != model.UserName)
                    _context.Entry(found).State = EntityState.Deleted;
                    _context.SaveChanges();
                }
                if(found == null || (found != null && found.UserName != model.UserName)) {
                    var _driverVehicle = new tblDriverVehicle
                    {
                        UserName = model.UserName,
                        Vehicle = model.Vehicle,
                        CreateDay = DateTime.Now,
                        UpdateDay= DateTime.Now,
                        CreateBy = model.UpdateBy,
                        UpdateBy = model.UpdateBy,
                    };

                    _context.tblDriverVehicle.Add(_driverVehicle);
                    await _context.SaveChangesAsync();
                }
            }

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