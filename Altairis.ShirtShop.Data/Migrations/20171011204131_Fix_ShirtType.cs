using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Altairis.ShirtShop.Data.Migrations
{
    public partial class Fix_ShirtType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShirtTypes_ShirtTypeIdId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShirtTypeIdId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShirtTypeIdId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "ShirtTypeId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShirtTypeId",
                table: "Orders",
                column: "ShirtTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShirtTypes_ShirtTypeId",
                table: "Orders",
                column: "ShirtTypeId",
                principalTable: "ShirtTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShirtTypes_ShirtTypeId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShirtTypeId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShirtTypeId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "ShirtTypeIdId",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShirtTypeIdId",
                table: "Orders",
                column: "ShirtTypeIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShirtTypes_ShirtTypeIdId",
                table: "Orders",
                column: "ShirtTypeIdId",
                principalTable: "ShirtTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
