using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedVendorNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SequentialNumber",
                table: "Vendors",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR VendorNumberSequence");

            migrationBuilder.AddColumn<string>(
                name: "VendorNumber",
                table: "Vendors",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true,
                computedColumnSql: "CONCAT('VEN', RIGHT('00000' + CAST([SequentialNumber] AS VARCHAR(5)), 5))",
                stored: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VendorNumber",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "SequentialNumber",
                table: "Vendors");
        }
    }
}
