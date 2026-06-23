using API.Data;
using DomainModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
	builder.Configuration.AddUserSecrets<Program>(optional: true);
}

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
			.WithOrigins("http://localhost:5123", "http://10.133.51.145:8080", "https://victoria.merchantec.tech")
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

app.UseSwagger();
app.UseSwaggerUI();
if (!app.Environment.IsProduction())
{
app.UseHttpsRedirection();
}
app.UseCors("AllowBlazorClient");
app.UseAuthorization();

app.MapControllers();

await InitializeDatabaseAsync(app);

static async Task InitializeDatabaseAsync(WebApplication webApp)
{
	// Number of times to retry before giving up (e.g. database container still starting).
	const int maxRetryAttempts = 10;
	var retryDelay = TimeSpan.FromSeconds(5);

	var logger = webApp.Services.GetRequiredService<ILogger<Program>>();

	for (var attempt = 1; attempt <= maxRetryAttempts; attempt++)
	{
		try
		{
			using var scope = webApp.Services.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			dbContext.Database.Migrate();

			logger.LogInformation("Database migration completed successfully.");
			return;
		}
		catch (Exception ex)
		{
			logger.LogWarning(
				ex,
				"Database initialization attempt {Attempt} of {MaxAttempts} failed. Retrying in {DelaySeconds}s...",
				attempt,
				maxRetryAttempts,
				retryDelay.TotalSeconds);

			if (attempt == maxRetryAttempts)
			{
				logger.LogError(ex, "Database initialization failed after {MaxAttempts} attempts.", maxRetryAttempts);
				throw;
			}

			await Task.Delay(retryDelay);
		}
	}
}

app.Run();
