using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLegacyDocumentationCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove rows whose Category string no longer maps to the current
            // DocumentationCategoryEnum (e.g. the old 'SshAndServerSecurity' values).
            // Otherwise EF throws when converting them back to the enum.
            migrationBuilder.Sql(@"
                DELETE FROM [DocumentationMilestones]
                WHERE [Category] NOT IN (
                    'ProjectDescription',
                    'InfrastructureAndDeployment',
                    'CiCdPipeline',
                    'Security',
                    'MonitoringAndOperations',
                    'LearningAndReflection'
                );");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // The deleted legacy rows cannot be restored.
        }
    }
}
