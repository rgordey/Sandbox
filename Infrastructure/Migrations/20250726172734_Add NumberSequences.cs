using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberSequences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "CustomerNumberSequence");

            migrationBuilder.CreateSequence<int>(
                name: "ProductNumberSequence");

            migrationBuilder.CreateSequence<int>(
                name: "PurchaseOrderNumberSequence");

            migrationBuilder.CreateSequence<int>(
                name: "SalesOrderNumberSequence");

            migrationBuilder.CreateSequence<int>(
                name: "VendorNumberSequence");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "CustomerNumberSequence");

            migrationBuilder.DropSequence(
                name: "ProductNumberSequence");

            migrationBuilder.DropSequence(
                name: "PurchaseOrderNumberSequence");

            migrationBuilder.DropSequence(
                name: "SalesOrderNumberSequence");

            migrationBuilder.DropSequence(
                name: "VendorNumberSequence");
        }
    }
}
