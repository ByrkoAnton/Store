using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Store.DataAccessLayer.Migrations
{
    public partial class entity_DateOfCreation_defaultValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfCreation",
                table: "PrintingEditions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfCreation",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfCreation",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfCreation",
                table: "OrderItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfCreation",
                table: "Authors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfCreation",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfCreation",
                table: "PrintingEditions");

            migrationBuilder.DropColumn(
                name: "DateOfCreation",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DateOfCreation",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DateOfCreation",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "DateOfCreation",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "DateOfCreation",
                table: "AspNetUsers");
        }
    }
}
