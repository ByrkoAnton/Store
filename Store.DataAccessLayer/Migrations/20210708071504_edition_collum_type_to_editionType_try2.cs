using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Store.DataAccessLayer.Migrations
{
    public partial class edition_collum_type_to_editionType_try2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "PrintingEditions",
                newName: "EditionType");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EditionType",
                table: "PrintingEditions",
                newName: "Type");

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateOfCreation",
                value: new DateTime(2021, 7, 8, 10, 6, 56, 813, DateTimeKind.Local).AddTicks(5760));

            migrationBuilder.UpdateData(
                table: "PrintingEditions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateOfCreation",
                value: new DateTime(2021, 7, 8, 10, 6, 56, 817, DateTimeKind.Local).AddTicks(3091));
        }
    }
}
