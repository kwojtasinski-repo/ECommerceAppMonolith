using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Modules.Sales.Infrastructure.EF.Migrations
{
    public partial class Sales_Module_AddedValueObjectsToEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                schema: "sales",
                table: "Orders",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Rate",
                schema: "sales",
                table: "Orders",
                type: "numeric(14,4)",
                precision: 14,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                schema: "sales",
                table: "OrderItems",
                type: "numeric(14,4)",
                precision: 14,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                schema: "sales",
                table: "OrderItems",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Rate",
                schema: "sales",
                table: "OrderItems",
                type: "numeric(14,4)",
                precision: 14,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                schema: "sales",
                table: "ItemSales",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                schema: "sales",
                table: "ItemCarts",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CurrencyCode",
                schema: "sales",
                table: "Orders",
                column: "CurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_CurrencyCode",
                schema: "sales",
                table: "OrderItems",
                column: "CurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSales_CurrencyCode",
                schema: "sales",
                table: "ItemSales",
                column: "CurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCarts_CurrencyCode",
                schema: "sales",
                table: "ItemCarts",
                column: "CurrencyCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_CurrencyCode",
                schema: "sales",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_CurrencyCode",
                schema: "sales",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_ItemSales_CurrencyCode",
                schema: "sales",
                table: "ItemSales");

            migrationBuilder.DropIndex(
                name: "IX_ItemCarts_CurrencyCode",
                schema: "sales",
                table: "ItemCarts");

            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                schema: "sales",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Rate",
                schema: "sales",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Cost",
                schema: "sales",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                schema: "sales",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Rate",
                schema: "sales",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                schema: "sales",
                table: "ItemSales");

            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                schema: "sales",
                table: "ItemCarts");
        }
    }
}
