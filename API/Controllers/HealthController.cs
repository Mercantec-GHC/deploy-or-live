using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
	private readonly AppDbContext _dbContext;
	private readonly IWebHostEnvironment _environment;

	public HealthController(AppDbContext dbContext, IWebHostEnvironment environment)
	{
		_dbContext = dbContext;
		_environment = environment;
	}

	[HttpGet]
	public async Task<IActionResult> GetHealth()
	{
		try
		{
			var canConnectToDatabase = await _dbContext.Database.CanConnectAsync();

			if (!canConnectToDatabase)
			{
				return StatusCode(503, new
				{
					status = "Unhealthy",
					database = "Disconnected",
					environment = _environment.EnvironmentName,
					timestamp = DateTime.UtcNow
				});
			}

			return Ok(new
			{
				status = "Healthy",
				database = "Connected",
				environment = _environment.EnvironmentName,
				timestamp = DateTime.UtcNow
			});
		}
		catch (Exception ex)
		{
			return StatusCode(503, new
			{
				status = "Unhealthy",
				database = "Disconnected",
				environment = _environment.EnvironmentName,
				error = ex.Message,
				timestamp = DateTime.UtcNow
			});
		}
	}
}