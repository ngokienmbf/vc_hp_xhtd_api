using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using XHTDHP_API.Data;
using XHTDHP_API.Entities;
using XHTDHP_API.Helpers;
using XHTDHP_API.Models.Filter;

namespace XHTDHP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderOperatingController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public OrderOperatingController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/driver
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var dayRequire = DateTime.Today.AddDays(-1);
            var query = _context.tblStoreOrderOperating.Where(o => o.OrderDate >= dayRequire).OrderByDescending(item => item.OrderDate).AsNoTracking();
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
            return Ok(found);
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

        [HttpGet]
        [Route("exportReport")]
        public async Task<IActionResult> ExportToExcel([FromQuery] PaginationFilter filter)
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
            var listOrder = await query.ToListAsync();
            var stream = new MemoryStream();
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("Bao-cao-don-hang");
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                namedStyle.Style.Font.Color.SetColor(Color.Blue);
                const int startRow = 5;
                var row = startRow;

                //Create Headers and format them
                worksheet.Cells["A1"].Value = "BÁO CÁO ĐƠN HÀNG";
                using (var r = worksheet.Cells["A1:J1"])
                {
                    r.Merge = true;
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                }

                worksheet.Cells["A4"].Value = "Ngày đặt hàng";
                worksheet.Cells["B4"].Value = "MSGH";
                worksheet.Cells["C4"].Value = "Biến số xe";
                worksheet.Cells["D4"].Value = "Mã RFID";
                worksheet.Cells["E4"].Value = "Họ tên lái xe";
                worksheet.Cells["F4"].Value = "Tên nhà phân phối";
                worksheet.Cells["G4"].Value = "Hàng hóa";
                worksheet.Cells["H4"].Value = "Khối lượng đặt";
                worksheet.Cells["I4"].Value = "Số thứ tự lấy hàng";
                worksheet.Cells["J4"].Value = "Trạng thái đơn hàng";
                worksheet.Cells["A4:J4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A4:J4"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                worksheet.Cells["A4:J4"].Style.Font.Bold = true;

                row = 5;
                foreach (var order in listOrder)
                {
                    worksheet.Cells[row, 1].Value = order.OrderDate;
                    worksheet.Cells[row, 2].Value = order.DeliveryCode;
                    worksheet.Cells[row, 3].Value = order.Vehicle;
                    worksheet.Cells[row, 4].Value = order.CardNo;
                    worksheet.Cells[row, 5].Value = order.DriverName;
                    worksheet.Cells[row, 6].Value = order.NameDistributor;
                    worksheet.Cells[row, 7].Value = order.NameProduct;
                    worksheet.Cells[row, 8].Value = order.SumNumber;
                    worksheet.Cells[row, 9].Value = order.IndexOrder;
                    worksheet.Cells[row, 10].Value = order.State;

                    row++;
                }

                // set some core property values
                xlPackage.Workbook.Properties.Title = "Bao-cao";
                xlPackage.Workbook.Properties.Author = "XMHP";
                xlPackage.Workbook.Properties.Subject = "Bao-cao";
                // save the new spreadsheet
                xlPackage.Save();
                // Response.Clear();
            }
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "bao-cao-don-hang.xlsx");
        }
    }
}