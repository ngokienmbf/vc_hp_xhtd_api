using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XHTDHP_API.Data;
using XHTDHP_API.Entities;
using XHTDHP_API.Helpers;
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
            var query = _context.Vehicles.OrderBy(item => item.LicensePlace).AsNoTracking();
            if (!String.IsNullOrEmpty(filter.SeachKey))
            {
                query = query.Where(item => item.LicensePlace.Contains(filter.SeachKey));
            }
            var totalRecords = await query.CountAsync();
            query = query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);
            var pagedData = await query.ToListAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Vehicle>(pagedData, filter, totalRecords);
            return Ok(pagedReponse);
        }

        
    }
}