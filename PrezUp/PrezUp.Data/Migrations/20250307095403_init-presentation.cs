using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrezUp.Data.Migrations
{
    /// <inheritdoc />
    public partial class initpresentation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Presentation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Clarity = table.Column<int>(type: "int", nullable: false),
                    ClarityFeedback = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fluency = table.Column<int>(type: "int", nullable: false),
                    FluencyFeedback = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Confidence = table.Column<int>(type: "int", nullable: false),
                    ConfidenceFeedback = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Engagement = table.Column<int>(type: "int", nullable: false),
                    EngagementFeedback = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpeechStyle = table.Column<int>(type: "int", nullable: false),
                    SpeechStyleFeedback = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Tips = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presentation", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Presentation");
        }
    }
}
