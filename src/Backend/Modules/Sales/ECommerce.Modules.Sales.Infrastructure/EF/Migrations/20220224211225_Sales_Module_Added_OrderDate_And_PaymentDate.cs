using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Modules.Sales.Infrastructure.EF.Migrations
{
    public partial class Sales_Module_Added_OrderDate_And_PaymentDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                schema: "sales",
                table: "OrderItems");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                schema: "sales",
                table: "Payments",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "sales",
                table: "Payments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateOrderDate",
                schema: "sales",
                table: "Orders",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderApprovedDate",
                schema: "sales",
                table: "Orders",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                schema: "sales",
                table: "OrderItems",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                schema: "sales",
                table: "OrderItems",
                column: "OrderId",
                principalSchema: "sales",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                schema: "sales",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                schema: "sales",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "sales",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CreateOrderDate",
                schema: "sales",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderApprovedDate",
                schema: "sales",
                table: "Orders");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                schema: "sales",
                table: "OrderItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                schema: "sales",
                table: "OrderItems",
                column: "OrderId",
                principalSchema: "sales",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
