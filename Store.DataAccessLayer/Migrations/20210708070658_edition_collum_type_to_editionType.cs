using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Store.DataAccessLayer.Migrations
{
    public partial class edition_collum_type_to_editionType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateOfCreation",
                value: new DateTime(2021, 5, 28, 10, 56, 51, 542, DateTimeKind.Local).AddTicks(1363));

            migrationBuilder.UpdateData(
                table: "PrintingEditions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateOfCreation",
                value: new DateTime(2021, 5, 28, 10, 56, 51, 545, DateTimeKind.Local).AddTicks(5881));
        }
    }
}
