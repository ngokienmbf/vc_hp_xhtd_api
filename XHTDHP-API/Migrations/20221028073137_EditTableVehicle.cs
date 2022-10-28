using Microsoft.EntityFrameworkCore.Migrations;

namespace XHTDHP_API.Migrations
{
    public partial class EditTableVehicle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DrivingLicence",
                table: "Vehicles",
                newName: "LicensePlace");

            migrationBuilder.AddColumn<string>(
                name: "DrivingLicense",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DrivingLicense",
                table: "Vehicles");

            migrationBuilder.RenameColumn(
                name: "LicensePlace",
                table: "Vehicles",
                newName: "DrivingLicence");
        }
    }
}
