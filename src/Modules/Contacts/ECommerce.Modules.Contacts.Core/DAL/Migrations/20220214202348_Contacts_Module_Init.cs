using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Modules.Contacts.Core.DAL.Migrations
{
    public partial class Contacts_Module_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "contacts");

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "contacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Company = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    NIP = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    InactivedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Inactived = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                schema: "contacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CityName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    StreetName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CountryName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ZipCode = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    BuildingNumber = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    LocaleNumber = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    InactivedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Inactived = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "contacts",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_BuildingNumber",
                schema: "contacts",
                table: "Addresses",
                column: "BuildingNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CustomerId",
                schema: "contacts",
                table: "Addresses",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_LocaleNumber",
                schema: "contacts",
                table: "Addresses",
                column: "LocaleNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ZipCode",
                schema: "contacts",
                table: "Addresses",
                column: "ZipCode");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_NIP",
                schema: "contacts",
                table: "Customers",
                column: "NIP",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PhoneNumber",
                schema: "contacts",
                table: "Customers",
                column: "PhoneNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses",
                schema: "contacts");

            migrationBuilder.DropTable(
                name: "Customers",
                schema: "contacts");
        }
    }
}
