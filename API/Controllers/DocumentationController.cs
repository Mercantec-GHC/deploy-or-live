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
	private readonly string _documentationEditApiKey;
	private const string EditApiKeyHeaderName = "X-Documentation-Edit-Key";

  public DocumentationController(AppDbContext dbContext, IConfiguration configuration)
	{
		_dbContext = dbContext;
      _documentationEditApiKey = configuration["DocumentationEditApiKey"]?.Trim()
			?? throw new InvalidOperationException("DocumentationEditApiKey is missing from configuration.");

		if (string.IsNullOrWhiteSpace(_documentationEditApiKey))
		{
			throw new InvalidOperationException("DocumentationEditApiKey is empty. Set a non-empty value (for development, use User Secrets).");
		}
	}

	[HttpGet]
	public async Task<ActionResult<List<DocumentationMilestoneDto>>> GetMilestones()
	{
		var savedMilestones = await _dbContext.DocumentationMilestones
			.ToDictionaryAsync(milestone => milestone.Category);

		var milestones = Enum.GetValues<DocumentationCategoryEnum>()
			.OrderBy(category => (int)category)
			.Select(category =>
			{
				savedMilestones.TryGetValue(category, out var saved);

				return new DocumentationMilestoneDto
				{
					Id = saved?.Id ?? 0,
					Category = category,
					CategoryDisplayName = string.IsNullOrWhiteSpace(saved?.CategoryDisplayName)
						? GetCategoryDisplayName(category)
						: saved.CategoryDisplayName,
					DocumentationContent = saved?.DocumentationContent ?? string.Empty,
					UpdatedAt = saved?.UpdatedAt ?? DateTime.UtcNow
				};
			})
			.ToList();

		return Ok(milestones);
	}

	[HttpGet("{category}")]
	public async Task<ActionResult<DocumentationMilestoneDto>> GetMilestoneByCategory(
		DocumentationCategoryEnum category)
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
	DocumentationCategoryEnum category,
	UpdateDocumentationMilestoneDto updateDto)
	{
     if (!Request.Headers.TryGetValue(EditApiKeyHeaderName, out var providedApiKey)
			|| !string.Equals(providedApiKey.ToString().Trim(), _documentationEditApiKey, StringComparison.Ordinal))
		{
			return Unauthorized("Invalid edit key.");
		}

		if (!Enum.IsDefined(category))
		{
			return BadRequest("Unknown documentation category.");
		}

		var milestone = await _dbContext.DocumentationMilestones
			.FirstOrDefaultAsync(milestone => milestone.Category == category);

		if (milestone is null)
		{
			milestone = new DocumentationMilestone { Category = category };
			_dbContext.DocumentationMilestones.Add(milestone);
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
	private static string GetCategoryDisplayName(DocumentationCategoryEnum category)
	{
		return category switch
		{
			DocumentationCategoryEnum.ProjectDescription => "Project Description",
			DocumentationCategoryEnum.InfrastructureAndDeployment => "Infrastructure & Deployment",
			DocumentationCategoryEnum.CiCdPipeline => "CI/CD Pipeline",
			DocumentationCategoryEnum.Security => "Security",
			DocumentationCategoryEnum.MonitoringAndOperations => "Monitoring & Operations",
			DocumentationCategoryEnum.LearningAndReflection => "Learning & Reflection",
			_ => category.ToString()
		};
	}
}