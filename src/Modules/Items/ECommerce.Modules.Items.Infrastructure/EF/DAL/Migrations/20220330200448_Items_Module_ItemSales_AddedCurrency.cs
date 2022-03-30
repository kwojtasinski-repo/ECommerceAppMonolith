using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Modules.Items.Infrastructure.EF.DAL.Migrations
{
    public partial class Items_Module_ItemSales_AddedCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                schema: "items",
                table: "ItemSales",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "PLN");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSales_CurrencyCode",
                schema: "items",
                table: "ItemSales",
                column: "CurrencyCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemSales_CurrencyCode",
                schema: "items",
                table: "ItemSales");

            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                schema: "items",
                table: "ItemSales");
        }
    }
}
