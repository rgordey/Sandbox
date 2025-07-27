using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerNumberIII : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CustomerNumber",
                table: "Customers",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true,
                computedColumnSql: "CONCAT('CUS', RIGHT('00000' + CAST([SequentialNumber] AS VARCHAR(5)), 5))",
                stored: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8,
                oldNullable: true,
                oldComputedColumnSql: "CONCAT('CUS', FORMAT([SequentialNumber], 'D5'))",
                oldStored: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CustomerNumber",
                table: "Customers",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true,
                computedColumnSql: "CONCAT('CUS', FORMAT([SequentialNumber], 'D5'))",
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8,
                oldNullable: true,
                oldComputedColumnSql: "CONCAT('CUS', RIGHT('00000' + CAST([SequentialNumber] AS VARCHAR(5)), 5))");
        }
    }
}
