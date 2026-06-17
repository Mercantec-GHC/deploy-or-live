using DomainModels.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<DeploymentNoteVm> DeploymentNotes => Set<DeploymentNoteVm>();

    public DbSet<DocumentationMilestone> DocumentationMilestones => Set<DocumentationMilestone>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DocumentationMilestone>()
            .Property(milestone => milestone.Category)
            .HasConversion<string>();

        modelBuilder.Entity<DocumentationMilestone>()
            .HasIndex(milestone => milestone.Category)
            .IsUnique();
    }
}