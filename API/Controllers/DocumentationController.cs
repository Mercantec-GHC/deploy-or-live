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
				DocumentationContent = milestone.DocumentationContent,
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
				DocumentationContent = milestone.DocumentationContent,
				UpdatedAt = milestone.UpdatedAt
			})
			.FirstOrDefaultAsync();

		if (milestone is null)
		{
			return NotFound();
		}

		return Ok(milestone);
	}

	[HttpPut("{category}")]
	public async Task<ActionResult<DocumentationMilestoneDto>> UpdateMilestone(
	DocumentationCategory category,
	UpdateDocumentationMilestoneDto updateDto)
	{
		// TODO: Protect this endpoint with authentication/authorization before production.
		var milestone = await _dbContext.DocumentationMilestones
			.FirstOrDefaultAsync(milestone => milestone.Category == category);

		if (milestone is null)
		{
			return NotFound();
		}

		milestone.CategoryDisplayName = updateDto.CategoryDisplayName;
		milestone.DocumentationContent = updateDto.DocumentationContent;
		milestone.UpdatedAt = DateTime.UtcNow;

		await _dbContext.SaveChangesAsync();

		return Ok(MapToDto(milestone));
	}
	private static DocumentationMilestoneDto MapToDto(DocumentationMilestone milestone)
	{
		return new DocumentationMilestoneDto
		{
			Id = milestone.Id,
			Category = milestone.Category,
			CategoryDisplayName = milestone.CategoryDisplayName,
			DocumentationContent = milestone.DocumentationContent,
			UpdatedAt = milestone.UpdatedAt
		};
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