using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace XHTDHP_API.Migrations
{
    public partial class CreateRFID2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropTable(
            //     name: "Drivers");

            // migrationBuilder.DropTable(
            //     name: "RFIDs");

            // migrationBuilder.DropTable(
            //     name: "Vehicles");

            migrationBuilder.CreateTable(
                name: "tblDriver",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdCard = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    State = table.Column<bool>(type: "bit", nullable: false),
                    CreateDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDriver", x => x.Id);
                });

            // migrationBuilder.CreateTable(
            //     name: "tblRFID",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         Vehicle = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         DayReleased = table.Column<DateTime>(type: "datetime2", nullable: false),
            //         DayExpired = table.Column<DateTime>(type: "datetime2", nullable: false),
            //         UserReleased = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         State = table.Column<bool>(type: "bit", nullable: false),
            //         Createday = table.Column<DateTime>(type: "datetime2", nullable: false),
            //         CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         UpdateDay = table.Column<DateTime>(type: "datetime2", nullable: false),
            //         UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         LastEnter = table.Column<DateTime>(type: "datetime2", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_tblRFID", x => x.Id);
            //     });

            migrationBuilder.CreateTable(
                name: "tblVehicle",
                columns: table => new
                {
                    IDVehicle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDStore = table.Column<int>(type: "int", nullable: false),
                    Vehicle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tonnage = table.Column<double>(type: "float", nullable: false),
                    TonnageDefault = table.Column<double>(type: "float", nullable: false),
                    NameDriver = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeightVehicle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WidthVehicle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LongVehicle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DayCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DayUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserCreate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnladenWeight1 = table.Column<int>(type: "int", nullable: false),
                    UnladenWeight2 = table.Column<int>(type: "int", nullable: false),
                    UnladenWeight3 = table.Column<int>(type: "int", nullable: false),
                    IsSetMediumUnladenWeight = table.Column<bool>(type: "bit", nullable: false),
                    CreateDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblVehicle", x => x.IDVehicle);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblDriver");

            migrationBuilder.DropTable(
                name: "tblRFID");

            migrationBuilder.DropTable(
                name: "tblVehicle");

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdCard = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    State = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RFIDs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DayExpired = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DayReleased = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastEnter = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<bool>(type: "bit", nullable: false),
                    Vehicle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RFIDs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DrivingLicense = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Height = table.Column<float>(type: "real", nullable: false),
                    LicensePlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NameDriver = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tonnage = table.Column<float>(type: "real", nullable: false),
                    TonnageDefault = table.Column<float>(type: "real", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });
        }
    }
}
