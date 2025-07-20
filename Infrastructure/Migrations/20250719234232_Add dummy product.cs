using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Adddummyproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[] { Guid.NewGuid(), "Dummy Product", 1.00m });

            // Then update OrderDetails
            migrationBuilder.Sql("UPDATE OrderDetails SET ProductId = (SELECT TOP 1 Id FROM Products) WHERE ProductId IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
