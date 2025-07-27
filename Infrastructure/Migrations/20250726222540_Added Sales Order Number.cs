using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedSalesOrderNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SequentialNumber",
                table: "SalesOrders",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR SalesOrderNumberSequence");

            migrationBuilder.AddColumn<string>(
                name: "OrderNumber",
                table: "SalesOrders",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true,
                computedColumnSql: "CONCAT('SO', RIGHT('000000' + CAST([SequentialNumber] AS VARCHAR(6)), 6))",
                stored: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "SequentialNumber",
                table: "SalesOrders");
        }
    }
}
