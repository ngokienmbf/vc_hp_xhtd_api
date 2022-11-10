using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
    public class CategoryController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public CategoryController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/driver
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var query = _context.tblCategories.OrderBy(item => item.ShowIndex).AsNoTracking();
            if (!String.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(item => item.Name.Contains(filter.Keyword));
            }
            var totalRecords = await query.CountAsync();
            query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);


            var pagedData = await query.ToListAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<tblCategories>(pagedData, filter, totalRecords);
            return Ok(pagedReponse);
        }


        [HttpGet("GetAllWithDevice")]
        public  IActionResult GetAllWithDevice([FromQuery] PaginationFilter filter)
        {
             var query =   _context.tblCategories.OrderBy(x => x.ShowIndex).AsNoTracking()    ;
            if (!String.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(item => item.Name.Contains(filter.Keyword));
            }
            var totalRecords = query.Count();
            var query2 = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            
            var pagedData =   query2.ToList()
                                 .GroupJoin(_context.tblCategoriesDevices,
                                            catory => catory.Code,
                                            catorydevice => catorydevice.CatCode,
                                            (catory, catoryDevice) => new CategoryDTO {
                                                Id = catory.Id,
                                                Code = catory.Code,
                                                Name = catory.Name, 
                                                State = catory.State, 
                                                ShowIndex = catory.ShowIndex, 
                                                Devices = catoryDevice.Select(i => new CategoryDTO.DeviceDTO {
                                                Code = i.Code,
                                                CatCode = i.CatCode,
                                                Name = i.Name, 
                                                State = i.State, 
                                                ShowIndex = i.ShowIndex }
                                                ).OrderBy(x => x.ShowIndex).ToList(),
                                            }).ToList();         

            //var pagedData = query2.ToList();
            var pagedReponse = PaginationHelper.CreatePagedReponse<CategoryDTO>(pagedData, filter, totalRecords);
            return Ok(pagedReponse);
        }
        public class CategoryDTO
        {
            public int Id { get; set; }
            public string Code { get; set; }
            public string? Name { get; set; }
            public bool State { get; set; } = true;
            public Nullable<int> ShowIndex { get; set; }
            public List<DeviceDTO> Devices {get; set;}
        public class DeviceDTO
        {
            public string Code { get; set; }
            public string CatCode { get; set; }
            public string? Name { get; set; }
            public bool State { get; set; } = true;
            public Nullable<int> ShowIndex { get; set; }
        }

        }

        [HttpGet("GetFull")]
        public async Task<IActionResult> GetFull()
        {
            var query = await _context.tblCategories.OrderBy(item => item.ShowIndex).ToListAsync();
            return Ok(query);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var found = await _context.tblCategories.Where(item => item.Id == id).FirstOrDefaultAsync();
            if (found == null)
            {
                return BadRequest("Không tìm thấy bản tin");
            }
            return Ok(found);
        }

        
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] tblCategories model)
        {
            model.CreateDay = DateTime.Now;
            _context.tblCategories.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new { succeeded = true, message = "Thêm thành công" });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] tblCategories model)
        {
            model.UpdateDay = DateTime.Now;
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { succeeded = true, message = "Cập nhật thành công", data = model });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var found = _context.tblCategories.Where(item => item.Id == id).FirstOrDefault();
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