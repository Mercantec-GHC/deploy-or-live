using API.Data;
using DomainModels.Dto;
using DomainModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentationController : ControllerBase
{
	private readonly AppDbContext _dbContext;

	public DocumentationController(AppDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	[HttpGet]
	public async Task<ActionResult<List<DocumentationMilestoneDto>>> GetMilestones()
	{
		var milestones = await _dbContext.DocumentationMilestones
			.OrderBy(milestone => milestone.Category)
			.Select(milestone => new DocumentationMilestoneDto
			{
				Id = milestone.Id,
				Category = milestone.Category,
				CategoryDisplayName = GetCategoryDisplayName(milestone.Category),
				WhatIDid = milestone.WhatIDid,
				CommandsUsed = milestone.CommandsUsed,
				ScreenshotNotes = milestone.ScreenshotNotes,
				ProblemsFaced = milestone.ProblemsFaced,
				WhatILearned = milestone.WhatILearned,
				ProductionConnection = milestone.ProductionConnection,
				UpdatedAt = milestone.UpdatedAt
			})
			.ToListAsync();

		return Ok(milestones);
	}

	[HttpGet("{category}")]
	public async Task<ActionResult<DocumentationMilestoneDto>> GetMilestoneByCategory(
		DocumentationCategory category)
	{
		var milestone = await _dbContext.DocumentationMilestones
			.Where(milestone => milestone.Category == category)
			.Select(milestone => new DocumentationMilestoneDto
			{
				Id = milestone.Id,
				Category = milestone.Category,
				CategoryDisplayName = GetCategoryDisplayName(milestone.Category),
				WhatIDid = milestone.WhatIDid,
				CommandsUsed = milestone.CommandsUsed,
				ScreenshotNotes = milestone.ScreenshotNotes,
				ProblemsFaced = milestone.ProblemsFaced,
				WhatILearned = milestone.WhatILearned,
				ProductionConnection = milestone.ProductionConnection,
				UpdatedAt = milestone.UpdatedAt
			})
			.FirstOrDefaultAsync();

		if (milestone is null)
		{
			return NotFound();
		}

		return Ok(milestone);
	}

	private static string GetCategoryDisplayName(DocumentationCategory category)
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
}