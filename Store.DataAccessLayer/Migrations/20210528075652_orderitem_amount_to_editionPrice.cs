using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Store.DataAccessLayer.Migrations
{
    public partial class orderitem_amount_to_editionPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "OrderItems",
                newName: "EditionPrice");

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
                columns: new[] { "DateOfCreation", "Title" },
                values: new object[] { new DateTime(2021, 5, 28, 10, 56, 51, 545, DateTimeKind.Local).AddTicks(5881), "FirstEdition" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EditionPrice",
                table: "OrderItems",
                newName: "Amount");

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateOfCreation",
                value: new DateTime(2021, 5, 26, 16, 58, 4, 240, DateTimeKind.Local).AddTicks(8074));

            migrationBuilder.UpdateData(
                table: "PrintingEditions",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "DateOfCreation", "Title" },
                values: new object[] { new DateTime(2021, 5, 26, 16, 58, 4, 247, DateTimeKind.Local).AddTicks(4586), null });
        }
    }
}
