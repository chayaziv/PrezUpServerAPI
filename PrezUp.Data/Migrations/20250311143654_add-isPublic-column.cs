﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrezUp.Data.Migrations
{
    /// <inheritdoc />
    public partial class addisPubliccolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Presentation",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Presentation");
        }
    }
}
