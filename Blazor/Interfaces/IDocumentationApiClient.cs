using DomainModels.Dto;
using DomainModels.Models;

namespace Blazor.Interfaces;

public interface IDocumentationApiClient
{
	Task<List<DocumentationMilestoneDto>> GetMilestonesAsync();

	Task<DocumentationMilestoneDto?> GetMilestoneByCategoryAsync(DocumentationCategory category);

	Task<DocumentationMilestoneDto?> UpdateMilestoneAsync(
		DocumentationCategory category,
		UpdateDocumentationMilestoneDto updateDto);
}