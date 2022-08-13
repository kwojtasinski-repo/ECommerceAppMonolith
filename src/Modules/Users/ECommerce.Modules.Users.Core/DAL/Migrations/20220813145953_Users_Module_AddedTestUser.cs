using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Modules.Users.Core.DAL.Migrations
{
    public partial class Users_Module_AddedTestUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "users",
                table: "Users",
                columns: new[] { "Id", "Claims", "CreatedAt", "Email", "IsActive", "Password", "Role" },
                values: new object[] { new Guid("e70b6db8-f77a-4ce7-833f-977617cf1873"), "{\"permissions\":[\"users\",\"items\",\"item-sale\",\"currencies\"]}", new DateTime(2022, 8, 13, 16, 59, 53, 177, DateTimeKind.Local).AddTicks(589), "admin@admin.com", true, "AQAAAAEAACcQAAAAEP/+MBJ+0Y0ditII5cclQrsBB8G7mJyZ+y3zBn0yfFoHiSF/RiZCWSdemZ+eQ70Vag==", "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "users",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e70b6db8-f77a-4ce7-833f-977617cf1873"));
        }
    }
}
