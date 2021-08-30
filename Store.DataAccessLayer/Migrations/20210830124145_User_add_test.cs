using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Store.DataAccessLayer.Migrations
{
    public partial class User_add_test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Transac",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateOfCreation",
                value: new DateTime(2021, 8, 30, 15, 41, 44, 340, DateTimeKind.Local).AddTicks(683));

            migrationBuilder.UpdateData(
                table: "PrintingEditions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateOfCreation",
                value: new DateTime(2021, 8, 30, 15, 41, 44, 343, DateTimeKind.Local).AddTicks(644));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Transac",
                table: "Payments");

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateOfCreation",
                value: new DateTime(2021, 7, 8, 10, 15, 3, 74, DateTimeKind.Local).AddTicks(3848));

            migrationBuilder.UpdateData(
                table: "PrintingEditions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateOfCreation",
                value: new DateTime(2021, 7, 8, 10, 15, 3, 77, DateTimeKind.Local).AddTicks(5084));
        }
    }
}
