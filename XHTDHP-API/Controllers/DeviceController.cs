using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
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
    public class DeviceController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DeviceController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/driver
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var query = _context.tblCategoriesDevices.OrderBy(item => item.ShowIndex).AsNoTracking();
            if (!String.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(item => item.Name.Contains(filter.Keyword));
            }
            var totalRecords = await query.CountAsync();
            query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            var pagedData = await query.ToListAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<tblCategoriesDevices>(pagedData, filter, totalRecords);
            // SqlConnection sqlCon = _context.Database.GetDbConnection() as SqlConnection;
            return Ok(pagedReponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var found = await _context.tblCategoriesDevices.Where(item => item.Id == id).FirstOrDefaultAsync();
            if (found == null)
            {
                return BadRequest("Không tìm thấy bản tin");
            }
            return Ok(found);
        }

        [HttpGet("GetFull")]
        public async Task<IActionResult> GetFull()
        {
            var query = await _context.tblCategoriesDevices.OrderBy(item => item.ShowIndex).ToListAsync();
            return Ok(query);
        }
        
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] tblCategoriesDevices model)
        {
            model.CreateDay = DateTime.Now;
            _context.tblCategoriesDevices.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new { succeeded = true, message = "Thêm thành công" });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] tblCategoriesDevices model)
        {
            model.UpdateDay = DateTime.Now;
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { succeeded = true, message = "Cập nhật thành công", data = model });
        }

        [HttpPut("UpdateState")]
        public  IActionResult UpdateState([FromBody] tblCategoriesDevices model)
        {
            var succeeded = true;
            var message = "Cập nhật thành công";
            var exception = "";
            SqlConnection sqlCon = _context.Database.GetDbConnection() as SqlConnection;
            try
            {
                sqlCon.Open();
                SqlCommand Cmd = sqlCon.CreateCommand();
                Cmd.CommandText = "update tblCategoriesDevices set State = @State Where Id = @Id";
                Cmd.Parameters.Add("State", SqlDbType.Bit).Value = model.State;
                Cmd.Parameters.Add("Id", SqlDbType.Int).Value = model.Id;
                Cmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                log.Info(Ex.Message);
                exception = Ex.Message;
                succeeded = false;
                message = "Cập nhật không thành công";
            }
            finally
            {
                sqlCon.Close();
                sqlCon.Dispose();
            }
            return Ok(new { succeeded = succeeded, message = message, data = model, exception =  exception});
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var found = _context.tblCategoriesDevices.Where(item => item.Id == id).FirstOrDefault();
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