using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Modules.Sales.Infrastructure.EF.Migrations
{
    public partial class Sales_Module_AddedCurrencyRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrencyRate",
                schema: "sales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Rate = table.Column<decimal>(type: "numeric(14,4)", precision: 14, scale: 4, nullable: false),
                    CurrencyCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    RateDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Created = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyRate", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyRate_CurrencyCode",
                schema: "sales",
                table: "CurrencyRate",
                column: "CurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyRate_CurrencyCode_Created",
                schema: "sales",
                table: "CurrencyRate",
                columns: new[] { "CurrencyCode", "Created" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyRate",
                schema: "sales");
        }
    }
}
