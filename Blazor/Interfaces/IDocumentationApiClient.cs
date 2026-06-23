using DomainModels.Dto;
using DomainModels.Models;

namespace Blazor.Interfaces;

public interface IDocumentationApiClient
{
	Task<List<DocumentationMilestoneDto>> GetMilestonesAsync();

	Task<DocumentationMilestoneDto?> GetMilestoneByCategoryAsync(DocumentationCategoryEnum category);

	Task<DocumentationMilestoneDto?> UpdateMilestoneAsync(
		DocumentationCategoryEnum category,
     UpdateDocumentationMilestoneDto updateDto,
		string editApiKey);
}