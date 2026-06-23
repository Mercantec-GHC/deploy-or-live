using DomainModels.Models;

namespace DomainModels.Dto;

public class DocumentationMilestoneDto
{
	public int Id { get; set; }
	public DocumentationCategoryEnum Category { get; set; }
	public string CategoryDisplayName { get; set; } = string.Empty;
	public string DocumentationContent { get; set; } = string.Empty;
	public DateTime UpdatedAt { get; set; }
}