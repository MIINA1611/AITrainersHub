using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AITrainersHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SupportBilingualContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "WorkOpportunities",
                newName: "DescriptionEn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Platforms",
                newName: "DescriptionEn");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "OnboardingSteps",
                newName: "TitleEn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "OnboardingSteps",
                newName: "DescriptionEn");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "WorkOpportunities",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Language",
                table: "VideoGuides",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "Platforms",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "OnboardingSteps",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TitleAr",
                table: "OnboardingSteps",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "WorkOpportunities");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "VideoGuides");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "Platforms");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "OnboardingSteps");

            migrationBuilder.DropColumn(
                name: "TitleAr",
                table: "OnboardingSteps");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "WorkOpportunities",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "Platforms",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "TitleEn",
                table: "OnboardingSteps",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "OnboardingSteps",
                newName: "Description");
        }
    }
}
