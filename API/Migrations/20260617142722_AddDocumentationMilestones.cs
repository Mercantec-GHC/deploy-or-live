using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentationMilestones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentationMilestones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WhatIDid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommandsUsed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScreenshotNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProblemsFaced = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhatILearned = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductionConnection = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentationMilestones", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentationMilestones_Category",
                table: "DocumentationMilestones",
                column: "Category",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentationMilestones");
        }
    }
}
