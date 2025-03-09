using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrezUp.Data.Migrations
{
    /// <inheritdoc />
    public partial class userpresentation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Presentation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Presentation_UserId",
                table: "Presentation",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Presentation_User_UserId",
                table: "Presentation",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Presentation_User_UserId",
                table: "Presentation");

            migrationBuilder.DropIndex(
                name: "IX_Presentation_UserId",
                table: "Presentation");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Presentation");
        }
    }
}
