using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProductVendor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVendor_Products_ProductId",
                table: "ProductVendor");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVendor_Vendors_VendorId",
                table: "ProductVendor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductVendor",
                table: "ProductVendor");

            migrationBuilder.RenameTable(
                name: "ProductVendor",
                newName: "ProductVendors");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVendor_VendorId",
                table: "ProductVendors",
                newName: "IX_ProductVendors_VendorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductVendors",
                table: "ProductVendors",
                columns: new[] { "ProductId", "VendorId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVendors_Products_ProductId",
                table: "ProductVendors",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVendors_Vendors_VendorId",
                table: "ProductVendors",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVendors_Products_ProductId",
                table: "ProductVendors");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVendors_Vendors_VendorId",
                table: "ProductVendors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductVendors",
                table: "ProductVendors");

            migrationBuilder.RenameTable(
                name: "ProductVendors",
                newName: "ProductVendor");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVendors_VendorId",
                table: "ProductVendor",
                newName: "IX_ProductVendor_VendorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductVendor",
                table: "ProductVendor",
                columns: new[] { "ProductId", "VendorId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVendor_Products_ProductId",
                table: "ProductVendor",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVendor_Vendors_VendorId",
                table: "ProductVendor",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
