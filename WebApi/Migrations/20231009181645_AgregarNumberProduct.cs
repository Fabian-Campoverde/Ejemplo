using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    public partial class AgregarNumberProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "NumberProducts",
                columns: table => new
                {
                    ProductNo = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Detalle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumberProducts", x => x.ProductNo);
                    table.ForeignKey(
                        name: "FK_NumberProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2023, 10, 9, 13, 16, 45, 152, DateTimeKind.Local).AddTicks(31));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2023, 10, 9, 13, 16, 45, 152, DateTimeKind.Local).AddTicks(43));

            migrationBuilder.CreateIndex(
                name: "IX_NumberProducts_ProductId",
                table: "NumberProducts",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NumberProducts");

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "Tipo" },
                values: new object[] { new DateTime(2023, 10, 8, 11, 9, 30, 650, DateTimeKind.Local).AddTicks(9302), "" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "Tipo" },
                values: new object[] { new DateTime(2023, 10, 8, 11, 9, 30, 650, DateTimeKind.Local).AddTicks(9318), "" });
        }
    }
}
