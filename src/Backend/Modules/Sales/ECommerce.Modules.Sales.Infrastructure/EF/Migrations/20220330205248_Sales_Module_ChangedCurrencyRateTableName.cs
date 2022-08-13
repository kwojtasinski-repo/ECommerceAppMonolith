using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Modules.Sales.Infrastructure.EF.Migrations
{
    public partial class Sales_Module_ChangedCurrencyRateTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrencyRate",
                schema: "sales",
                table: "CurrencyRate");

            migrationBuilder.RenameTable(
                name: "CurrencyRate",
                schema: "sales",
                newName: "CurrencyRates",
                newSchema: "sales");

            migrationBuilder.RenameIndex(
                name: "IX_CurrencyRate_CurrencyCode_Created",
                schema: "sales",
                table: "CurrencyRates",
                newName: "IX_CurrencyRates_CurrencyCode_Created");

            migrationBuilder.RenameIndex(
                name: "IX_CurrencyRate_CurrencyCode",
                schema: "sales",
                table: "CurrencyRates",
                newName: "IX_CurrencyRates_CurrencyCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrencyRates",
                schema: "sales",
                table: "CurrencyRates",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrencyRates",
                schema: "sales",
                table: "CurrencyRates");

            migrationBuilder.RenameTable(
                name: "CurrencyRates",
                schema: "sales",
                newName: "CurrencyRate",
                newSchema: "sales");

            migrationBuilder.RenameIndex(
                name: "IX_CurrencyRates_CurrencyCode_Created",
                schema: "sales",
                table: "CurrencyRate",
                newName: "IX_CurrencyRate_CurrencyCode_Created");

            migrationBuilder.RenameIndex(
                name: "IX_CurrencyRates_CurrencyCode",
                schema: "sales",
                table: "CurrencyRate",
                newName: "IX_CurrencyRate_CurrencyCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrencyRate",
                schema: "sales",
                table: "CurrencyRate",
                column: "Id");
        }
    }
}
