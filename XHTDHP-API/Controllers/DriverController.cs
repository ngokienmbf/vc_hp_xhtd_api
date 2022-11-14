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
            var query = _context.tblDriver.OrderBy(item => item.Id).AsNoTracking();
            if (!String.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(item => item.FullName.Contains(filter.Keyword));
            }
            var totalRecords = await query.CountAsync();
            query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            var pagedData = await query.ToListAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<tblDriver>(pagedData, filter, totalRecords);
            return Ok(pagedReponse);
        }

        [HttpGet("GetFull")]
        public async Task<IActionResult> GetFull()
        {
            var query = await _context.tblDriver.OrderBy(item => item.UserName).ToListAsync();
            return Ok(query);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var driver = await _context.tblDriver.FindAsync(id);
            return Ok(driver);
        }

        [HttpGet("GetWithVehicles/{id}")]
        public async Task<IActionResult> GetWithVehicles(int id)
        {
            var driver = await _context.tblDriver.FindAsync(id);
            var found = await _context.tblDriverVehicle.Where(item => item.UserName==driver.UserName).Select(item => item.Vehicle).ToListAsync();
            driver.Vehicles = found;
            return Ok(driver);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] tblDriver model)
        {
            _context.tblDriver.Add(model);
            await _context.SaveChangesAsync();
            var newAccount = new tblAccount 
            {
                UserName = model.UserName,
                Password = RandomNumber(),
                GroupId = 1,
                State = true,
                CreateDay = DateTime.Now
            };
            await _context.tblAccount.AddAsync(newAccount);
            await _context.SaveChangesAsync();
            return Ok(new { succeeded = true, message = "Thêm thành công", statusCode = 200 });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] tblDriver model)
        {
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { succeeded = true, message = "Cập nhật thành công", data = model, statusCode = 200 });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var driver = _context.tblDriver.Find(id);
            if (driver != null)
            {
                 _context.Entry(driver).State = EntityState.Deleted;
                //xoa bang trung gian
                var found = await _context.tblDriverVehicle.Where(item => item.UserName == driver.UserName).ToListAsync();
                
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

        private static string RandomNumber()
        {
            Random rnd = new Random();
            var result = rnd.Next(0, 1000000).ToString("D6");
            return result;
        }
    }
}