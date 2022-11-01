using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using XHTDHP_API.Entities;

namespace XHTDHP_API.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        public DbSet<tblDriver> tblDriver { get; set; }
        public DbSet<tblVehicle> tblVehicle { get; set; }
        public DbSet<tblRFID> tblRFID { get; set; }
        public DbSet<tblAccount> tblAccount { get; set; }
        public DbSet<tblStoreOrderOperating> tblStoreOrderOperating { get; set; }
        public DbSet<tblDriverVehicle> tblDriverVehicle { get; set; }
    }
}