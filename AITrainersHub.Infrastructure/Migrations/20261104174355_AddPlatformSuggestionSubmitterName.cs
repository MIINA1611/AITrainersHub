using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AITrainersHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPlatformSuggestionSubmitterName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubmitterName",
                table: "PlatformSuggestions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubmitterName",
                table: "PlatformSuggestions");
        }
    }
}
