using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Modules.Items.Infrastructure.EF.DAL.Migrations
{
    public partial class Items_Module_ItemModification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemSales_ItemId",
                schema: "items",
                table: "ItemSales");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSales_ItemId",
                schema: "items",
                table: "ItemSales",
                column: "ItemId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemSales_ItemId",
                schema: "items",
                table: "ItemSales");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSales_ItemId",
                schema: "items",
                table: "ItemSales",
                column: "ItemId");
        }
    }
}
