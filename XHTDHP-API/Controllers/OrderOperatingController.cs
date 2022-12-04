using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using XHTDHP_API.Bussiness;
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
            var dayRequire = DateTime.Today.AddDays(-3);
            var query = _context.tblStoreOrderOperating.OrderByDescending(item => item.IndexOrder).ThenByDescending(x => x.Id).AsNoTracking();
            if (!String.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(item => item.Vehicle.Contains(filter.Keyword));
            }
            if (!String.IsNullOrEmpty(filter.DeliveryCode))
            {
                query = query.Where(item => item.DeliveryCode.Contains(filter.DeliveryCode));
            }
            if (!String.IsNullOrEmpty(filter.Step))
            {
                query = query.Where(item => item.Step == Int32.Parse(filter.Step));
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
        public async Task<IActionResult> ExportToExcel([FromQuery] string typeReport)
        {
            var dayRequire = DateTime.Today.AddDays(-3);
            var query = _context.tblStoreOrderOperating.Where(o => o.OrderDate >= dayRequire).OrderByDescending(item => item.OrderDate).AsNoTracking();
            var listOrder = await query.ToListAsync();
            var stream = new MemoryStream();
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("Bao-cao-don-hang");
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                namedStyle.Style.Font.Color.SetColor(Color.Blue);
                const int startRow = 7;
                var row = startRow;

                switch (typeReport)
                {
                    case "door":
                        //Create Headers and format them
                        worksheet.Cells["A1"].Value = "CÔNG TY TNHH MTV XI MĂNG VICEM HẢI PHÒNG BỘ PHẬN BẢO VỆ";
                        worksheet.Cells["A3"].Value = "BÁO CÁO TỔNG HỢP VÀO, RA CỔNG";
                        worksheet.Cells["A4"].Value = "(Từ ngày ../../…. đến ngày ../../….)";

                        using (var r = worksheet.Cells["A1:G1"])
                        {
                            r.Merge = true;
                            r.Style.Font.Color.SetColor(Color.White);
                            r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                            r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                        }
                        using (var r = worksheet.Cells["A3:G3"])
                        {
                            r.Merge = true;
                            r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                        }
                        using (var r = worksheet.Cells["A4:G4"])
                        {
                            r.Merge = true;
                            r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                        }
                        worksheet.Cells["A6"].Value = "Ngày đặt hàng";
                        worksheet.Cells["B6"].Value = "MSGH";
                        worksheet.Cells["C6"].Value = "Biến số xe";
                        worksheet.Cells["D6"].Value = "Vào cổng";
                        worksheet.Cells["E6"].Value = "Ra cổng";
                        worksheet.Cells["F6"].Value = "NPP/Khách hàng";
                        worksheet.Cells["G6"].Value = "Số lượng";
                        worksheet.Cells["A6:G6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells["A6:G6"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                        worksheet.Cells["A6:G6"].Style.Font.Bold = true;

                        row = 7;
                        foreach (var order in listOrder)
                        {
                            worksheet.Cells[row, 1].Value = order.OrderDate;
                            worksheet.Cells[row, 2].Value = order.DeliveryCode;
                            worksheet.Cells[row, 3].Value = order.Vehicle;
                            worksheet.Cells[row, 4].Value = order.TimeConfirm2;
                            worksheet.Cells[row, 5].Value = order.TimeConfirm8;
                            worksheet.Cells[row, 6].Value = order.NameDistributor;
                            worksheet.Cells[row, 7].Value = order.SumNumber;
                            row++;
                        }
                        break;
                    case "listorder":
                        //Create Headers and format them
                        worksheet.Cells["A1"].Value = "CÔNG TY TNHH MTV XI MĂNG VICEM HẢI BỘ PHẬN ĐIỀU HÀNH";
                        worksheet.Cells["A3"].Value = "BÁO CÁO TỔNG HỢP CẤP SỐ THỨ TỰ";
                        worksheet.Cells["A4"].Value = "(Từ ngày ../../…. đến ngày ../../….)";
                        worksheet.Cells["A3:I3"].Merge = true;
                        worksheet.Cells["A4:I4"].Merge = true;
                        using (var r = worksheet.Cells["A1:I1"])
                        {
                            r.Merge = true;
                            r.Style.Font.Color.SetColor(Color.White);
                            r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                            r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                        }
                        using (var r = worksheet.Cells["A3:I3"])
                        {
                            r.Merge = true;
                            r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                        }
                        using (var r = worksheet.Cells["A4:I4"])
                        {
                            r.Merge = true;
                            r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                        }
                        worksheet.Cells["A6"].Value = "Ngày đặt hàng";
                        worksheet.Cells["B6"].Value = "MSGH";
                        worksheet.Cells["C6"].Value = "Biến số xe";
                        worksheet.Cells["D6"].Value = "Vào cổng";
                        worksheet.Cells["E6"].Value = "Ra cổng";
                        worksheet.Cells["F6"].Value = "NPP/Khách hàng";
                        worksheet.Cells["G6"].Value = "Hàng hóa";
                        worksheet.Cells["H6"].Value = "Số lượng";
                        worksheet.Cells["I6"].Value = "Số thứ tự";
                        worksheet.Cells["A6:I6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells["A6:I6"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                        worksheet.Cells["A6:I6"].Style.Font.Bold = true;

                        row = 7;
                        foreach (var order in listOrder)
                        {
                            worksheet.Cells[row, 1].Value = order.OrderDate;
                            worksheet.Cells[row, 2].Value = order.DeliveryCode;
                            worksheet.Cells[row, 3].Value = order.Vehicle;
                            worksheet.Cells[row, 4].Value = order.TimeConfirm2;
                            worksheet.Cells[row, 5].Value = order.TimeConfirm8;
                            worksheet.Cells[row, 6].Value = order.NameDistributor;
                            worksheet.Cells[row, 7].Value = order.NameProduct;
                            worksheet.Cells[row, 8].Value = order.SumNumber;
                            worksheet.Cells[row, 8].Value = order.IndexOrder;
                            row++;
                        }
                        break;
                    case "weightStation":
                        //Create Headers and format them
                        worksheet.Cells["A1"].Value = "CÔNG TY TNHH MTV XI MĂNG VICEM HẢI PHÒNG";
                        worksheet.Cells["A2"].Value = "TRẠM CÂN 951";
                        worksheet.Cells["A3"].Value = "BÁO CÁO TỔNG HỢP CÂN HÀNG";
                        worksheet.Cells["A4"].Value = "(Từ ngày ../../…. đến ngày ../../….)";
                        worksheet.Cells["A3:K3"].Merge = true;
                        worksheet.Cells["A4:K4"].Merge = true;
                        using (var r = worksheet.Cells["A1:K1"])
                        {
                            r.Merge = true;
                            r.Style.Font.Color.SetColor(Color.White);
                            r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                            r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                        }
                        using (var r = worksheet.Cells["A3:K3"])
                        {
                            r.Merge = true;
                            r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                        }
                        using (var r = worksheet.Cells["A4:K4"])
                        {
                            r.Merge = true;
                            r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                        }
                        worksheet.Cells["A6"].Value = "Ngày đặt hàng";
                        worksheet.Cells["B6"].Value = "MSGH";
                        worksheet.Cells["C6"].Value = "Biến số xe";
                        worksheet.Cells["D6"].Value = "Vào cổng";
                        worksheet.Cells["E6"].Value = "Ra cổng";
                        worksheet.Cells["F6"].Value = "NPP/Khách hàng";
                        worksheet.Cells["G6"].Value = "Hàng hóa";
                        worksheet.Cells["H6"].Value = "Số lượng";
                        worksheet.Cells["I6"].Value = "KL bì";
                        worksheet.Cells["J6"].Value = "KL tổng";
                        worksheet.Cells["K6"].Value = "KL hàng";
                        worksheet.Cells["A6:K6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells["A6:K6"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                        worksheet.Cells["A6:K6"].Style.Font.Bold = true;

                        row = 7;
                        foreach (var order in listOrder)
                        {
                            worksheet.Cells[row, 1].Value = order.OrderDate;
                            worksheet.Cells[row, 2].Value = order.DeliveryCode;
                            worksheet.Cells[row, 3].Value = order.Vehicle;
                            worksheet.Cells[row, 4].Value = order.TimeConfirm2;
                            worksheet.Cells[row, 5].Value = order.TimeConfirm8;
                            worksheet.Cells[row, 6].Value = order.NameDistributor;
                            worksheet.Cells[row, 7].Value = order.NameProduct;
                            worksheet.Cells[row, 8].Value = order.SumNumber;
                            worksheet.Cells[row, 9].Value = order.SumNumber;
                            worksheet.Cells[row, 10].Value = order.SumNumber;
                            worksheet.Cells[row, 11].Value = order.SumNumber;
                            row++;
                        }
                        break;
                    default:
                        break;
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

        [HttpGet]
        [Route("getOrderByCode")]
        public async Task<IActionResult> GetOrderByCode([FromQuery] string code) 
        {
            var found = await _context.tblStoreOrderOperating.Where(o => o.DeliveryCode == code).FirstOrDefaultAsync();
            if (found == null)
            {
                return BadRequest(new {error = "Không tìm thấy order"});
            }
            return Ok(new {data = found, statusCode = 200});
        }

        [HttpGet]
        [Route("getOrderByRfid/{rfid}")]
        public async Task<IActionResult> GetOrderByRfid(string rfid)
        {
            var founds = await _context.tblStoreOrderOperating.Where(o => o.CardNo == rfid && (o.Step == 2 || o.Step == 1))
                .ToListAsync();
            var foudVehicle = await _context.tblRFID.Where(r => r.Code == rfid).FirstOrDefaultAsync();
            if (founds.Any(f => f == null) && foudVehicle == null) 
            {
                return BadRequest(new { error = "Không tìm thấy dữ liệu" });
            }
            return Ok(new { data = founds, vehicle = foudVehicle.Vehicle, statusCode = 200 });
        }

        [HttpGet]
        [Route("getOrderEnterExit")]
        public async Task<IActionResult> getOrderEnterExit([FromQuery] PaginationFilter filter)
        {
            var dayRequire = DateTime.Today.AddDays(-3);
            var query = _context.tblStoreOrderOperating.Where(o => o.OrderDate >= dayRequire && o.Step <= 6)
                .OrderByDescending(item => item.OrderDate).AsNoTracking();
            if (!String.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(item => item.Vehicle.Contains(filter.Keyword));
            }
            if (!String.IsNullOrEmpty(filter.DeliveryCode))
            {
                query = query.Where(item => item.DeliveryCode.Contains(filter.DeliveryCode));
            }

            var totalRecords = await query.CountAsync();
            query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            var pagedData = await query.ToListAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<tblStoreOrderOperating>(pagedData, filter, totalRecords);
            return Ok(pagedReponse);
        }
    }
}