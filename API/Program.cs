using API.Data;
using DomainModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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
builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("DocumentationEditKey", new OpenApiSecurityScheme
	{
		Description = "API key needed to edit documentation. Enter only the key value.",
		Type = SecuritySchemeType.ApiKey,
		Name = "X-Documentation-Edit-Key",
		In = ParameterLocation.Header
	});

	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "DocumentationEditKey"
				}
			},
			Array.Empty<string>()
		}
	});
});

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

	foreach (DocumentationCategory category in Enum.GetValues(typeof(DocumentationCategory)))
	{
		var milestoneExists = dbContext.DocumentationMilestones
			.Any(milestone => milestone.Category == category);

		if (!milestoneExists)
		{
			dbContext.DocumentationMilestones.Add(new DocumentationMilestone
			{
				Category = category,
				CategoryDisplayName = GetDefaultCategoryDisplayName(category),
				DocumentationContent = string.Empty,
				UpdatedAt = DateTime.UtcNow
			});
		}
	}

	dbContext.SaveChanges();
}

static string GetDefaultCategoryDisplayName(DocumentationCategory category)
{
	return category switch
	{
		DocumentationCategory.SshAndServerSecurity => "SSH & Server Security",
		DocumentationCategory.DnsFirewallAndDomain => "DNS, Firewall & Domain",
		DocumentationCategory.DatabaseSetup => "Database Setup",
		DocumentationCategory.NginxHttpsAndReverseProxy => "Nginx, HTTPS & Reverse Proxy",
		DocumentationCategory.DockerFundamentals => "Docker Fundamentals",
		DocumentationCategory.DockerCompose => "Docker Compose",
		DocumentationCategory.VolumesPersistenceAndNetworking => "Volumes, Persistence & Networking",
		DocumentationCategory.DokployGithubAndCiCd => "Dokploy, GitHub & CI/CD",
		DocumentationCategory.MonitoringAndLogging => "Monitoring & Logging",
		DocumentationCategory.OwaspAndSecurityHeaders => "OWASP & Security Headers",
		DocumentationCategory.ContainerSecurityAndSecrets => "Container Security & Secrets",
		_ => category.ToString()
	};
}

app.Run();
