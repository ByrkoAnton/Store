using Microsoft.EntityFrameworkCore.Migrations;

namespace Store.DataAccessLayer.Migrations
{
    public partial class Test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrintingEditions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prise = table.Column<double>(type: "float", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrintingEditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrintingEditions_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuthorPrintingEditions",
                columns: table => new
                {
                    AuthorId = table.Column<long>(type: "bigint", nullable: false),
                    PrintingEditionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorPrintingEditions", x => new { x.AuthorId, x.PrintingEditionId });
                    table.ForeignKey(
                        name: "FK_AuthorPrintingEditions_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorPrintingEditions_PrintingEditions_PrintingEditionId",
                        column: x => x.PrintingEditionId,
                        principalTable: "PrintingEditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Tolstoy" },
                    { 2L, "Gogol" },
                    { 3L, "Pushkin" }
                });

            migrationBuilder.InsertData(
                table: "PrintingEditions",
                columns: new[] { "Id", "AuthorId", "Currency", "Description", "IsRemoved", "Prise", "Status", "Type" },
                values: new object[,]
                {
                    { 1L, null, 6, "Anna Karenina", false, 5.0, "Avalible", 1 },
                    { 2L, null, 6, "Kosaks", false, 5.0, "Avalible", 1 },
                    { 3L, null, 6, "Diablo", false, 4.0, "Avalible", 1 },
                    { 4L, null, 5, "The Captain's Daughter", false, 5.0, "Avalible", 1 },
                    { 5L, null, 6, "Eugene Onegin", false, 5.0, "Avalible", 1 }
                });

            migrationBuilder.InsertData(
                table: "AuthorPrintingEditions",
                columns: new[] { "AuthorId", "PrintingEditionId" },
                values: new object[,]
                {
                    { 1L, 1L },
                    { 2L, 1L },
                    { 1L, 2L },
                    { 1L, 3L },
                    { 3L, 4L },
                    { 3L, 5L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorPrintingEditions_PrintingEditionId",
                table: "AuthorPrintingEditions",
                column: "PrintingEditionId");

            migrationBuilder.CreateIndex(
                name: "IX_PrintingEditions_AuthorId",
                table: "PrintingEditions",
                column: "AuthorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorPrintingEditions");

            migrationBuilder.DropTable(
                name: "PrintingEditions");

            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
