using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Altairis.ShirtShop.Data.Migrations
{
    public partial class Add_SortPriority : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortPriority",
                table: "ShirtSizes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SortPriority",
                table: "ShirtModels",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SortPriority",
                table: "DeliveryMethods",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortPriority",
                table: "ShirtSizes");

            migrationBuilder.DropColumn(
                name: "SortPriority",
                table: "ShirtModels");

            migrationBuilder.DropColumn(
                name: "SortPriority",
                table: "DeliveryMethods");
        }
    }
}
