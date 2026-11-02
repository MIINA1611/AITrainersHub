using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AITrainersHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkOpportunityPlatform : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PlatformId",
                table: "WorkOpportunities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WorkOpportunities_PlatformId",
                table: "WorkOpportunities",
                column: "PlatformId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOpportunities_Platforms_PlatformId",
                table: "WorkOpportunities",
                column: "PlatformId",
                principalTable: "Platforms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkOpportunities_Platforms_PlatformId",
                table: "WorkOpportunities");

            migrationBuilder.DropIndex(
                name: "IX_WorkOpportunities_PlatformId",
                table: "WorkOpportunities");

            migrationBuilder.DropColumn(
                name: "PlatformId",
                table: "WorkOpportunities");
        }
    }
}
