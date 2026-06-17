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
            new DeploymentNoteVm
            {
                Title = "SSH Basics",
                Category = "Linux",
                Content = "I learned how to connect to a remote Linux server using SSH.",
                CreatedAt = DateTime.UtcNow
            },
            new DeploymentNoteVm
            {
                Title = "Docker Containers",
                Category = "Docker",
                Content = "I learned that containers run isolated processes based on images.",
                CreatedAt = DateTime.UtcNow
            },
            new DeploymentNoteVm
            {
                Title = "Docker Volumes",
                Category = "Docker",
                Content = "I learned that volumes keep data even if containers are deleted.",
                CreatedAt = DateTime.UtcNow
            },
            new DeploymentNoteVm
            {
                Title = "Nginx Reverse Proxy",
                Category = "Networking",
                Content = "I learned that Nginx can receive traffic and forward it to my application.",
                CreatedAt = DateTime.UtcNow
            },
            new DeploymentNoteVm
            {
                Title = "Database Connection",
                Category = "Database",
                Content = "I learned that the backend connects to SQL Server using a connection string.",
                CreatedAt = DateTime.UtcNow
            }
        );

        dbContext.SaveChanges();
    }
}

app.Run();
