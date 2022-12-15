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
        public DbSet<tblRFIDSign> tblRFIDSign { get; set; }
        public DbSet<tblCategoriesDevices> tblCategoriesDevices { get; set; }
        public DbSet<tblCategories> tblCategories { get; set; }
        public DbSet<tblTrough> tblTrough { get; set; }
        public DbSet<tblTroughTypeProduct> tblTroughTypeProduct { get; set; }
        public DbSet<tblSystemParameter> tblSystemParameter { get; set; }
        public DbSet<tblAccountGroup> tblAccountGroup { get; set; }
        public DbSet<tblAccountGroupFunction> tblAccountGroupFunction { get; set; }
        public DbSet<tblFunction> tblFunction { get; set; }
    }
}