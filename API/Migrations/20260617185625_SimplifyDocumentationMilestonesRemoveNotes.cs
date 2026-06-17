using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyDocumentationMilestonesRemoveNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeploymentNotes");

            migrationBuilder.DropColumn(
                name: "CommandsUsed",
                table: "DocumentationMilestones");

            migrationBuilder.DropColumn(
                name: "ProblemsFaced",
                table: "DocumentationMilestones");

            migrationBuilder.DropColumn(
                name: "ProductionConnection",
                table: "DocumentationMilestones");

            migrationBuilder.DropColumn(
                name: "ScreenshotNotes",
                table: "DocumentationMilestones");

            migrationBuilder.RenameColumn(
                name: "WhatILearned",
                table: "DocumentationMilestones",
                newName: "DocumentationContent");

            migrationBuilder.RenameColumn(
                name: "WhatIDid",
                table: "DocumentationMilestones",
                newName: "CategoryDisplayName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DocumentationContent",
                table: "DocumentationMilestones",
                newName: "WhatILearned");

            migrationBuilder.RenameColumn(
                name: "CategoryDisplayName",
                table: "DocumentationMilestones",
                newName: "WhatIDid");

            migrationBuilder.AddColumn<string>(
                name: "CommandsUsed",
                table: "DocumentationMilestones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProblemsFaced",
                table: "DocumentationMilestones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductionConnection",
                table: "DocumentationMilestones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ScreenshotNotes",
                table: "DocumentationMilestones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DeploymentNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeploymentNotes", x => x.Id);
                });
        }
    }
}
