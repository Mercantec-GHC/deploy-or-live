using API.Data;
using DomainModels.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowBlazorClient", policy =>
	{
		policy
			.WithOrigins("http://localhost:5123")
			.AllowAnyHeader()
			.AllowAnyMethod();
	});
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazorClient");
app.UseAuthorization();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

	if (!dbContext.DeploymentNotes.Any())
	{
		dbContext.DeploymentNotes.AddRange(
		// old note seed data
		);

		dbContext.SaveChanges();
	}

	foreach (DocumentationCategory category in Enum.GetValues(typeof(DocumentationCategory)))
	{
		var milestoneExists = dbContext.DocumentationMilestones
			.Any(milestone => milestone.Category == category);

		if (!milestoneExists)
		{
			dbContext.DocumentationMilestones.Add(new DocumentationMilestone
			{
				Category = category,
				WhatIDid = string.Empty,
				CommandsUsed = string.Empty,
				ScreenshotNotes = string.Empty,
				ProblemsFaced = string.Empty,
				WhatILearned = string.Empty,
				ProductionConnection = string.Empty,
				UpdatedAt = DateTime.UtcNow
			});
		}
	}

	dbContext.SaveChanges();
}

app.Run();
