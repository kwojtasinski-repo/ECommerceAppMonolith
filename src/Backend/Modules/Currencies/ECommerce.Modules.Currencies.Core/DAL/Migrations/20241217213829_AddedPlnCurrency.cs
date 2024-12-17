using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Modules.Currencies.Core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedPlnCurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "currencies",
                table: "Currencies",
                columns: new[] { "Id", "Code", "Description" },
                values: new object[] { new Guid("77657829-cfb1-4603-858e-e1b66477a8e9"), "PLN", "Polski złoty" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "currencies",
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("77657829-cfb1-4603-858e-e1b66477a8e9"));
        }
    }
}
