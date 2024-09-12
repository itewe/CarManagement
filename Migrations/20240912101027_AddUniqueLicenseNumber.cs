using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueLicenseNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Drivers_LicenseNumber",
                table: "Drivers",
                column: "LicenseNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Drivers_LicenseNumber",
                table: "Drivers");
        }
    }
}
