using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    public partial class InsertarDatos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Tipo",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "FechaCreacion", "Name", "Precio", "Proveedor", "Stock", "Tipo", "imgUrl" },
                values: new object[] { 1, "Bebidas", "", new DateTime(2023, 10, 8, 11, 9, 30, 650, DateTimeKind.Local).AddTicks(9302), "Gaseosa", 15.5, "Coca Cola", 20, "", "" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "FechaCreacion", "Name", "Precio", "Proveedor", "Stock", "Tipo", "imgUrl" },
                values: new object[] { 2, "Bebidas", "", new DateTime(2023, 10, 8, 11, 9, 30, 650, DateTimeKind.Local).AddTicks(9318), "Agua", 5.5, "Coca Cola", 120, "", "" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AlterColumn<int>(
                name: "Tipo",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
