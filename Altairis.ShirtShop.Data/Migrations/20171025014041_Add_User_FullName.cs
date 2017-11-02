using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Altairis.ShirtShop.Data.Migrations {
public partial class Add_User_FullName : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
        migrationBuilder.AddColumn<string>(
            name: "FullName",
            table: "AspNetUsers",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: false,
            defaultValue: "");
        migrationBuilder.Sql("UPDATE AspNetUsers SET FullName = UserName");
    }

    protected override void Down(MigrationBuilder migrationBuilder) {
        migrationBuilder.DropColumn(
            name: "FullName",
            table: "AspNetUsers");
    }
}
}
