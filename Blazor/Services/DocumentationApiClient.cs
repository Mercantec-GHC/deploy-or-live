using System.Net.Http.Json;
using Blazor.Interfaces;
using DomainModels.Dto;
using DomainModels.Models;

namespace Blazor.Services;

public class DocumentationApiClient : IDocumentationApiClient
{
	private readonly HttpClient _httpClient;

	public DocumentationApiClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<List<DocumentationMilestoneDto>> GetMilestonesAsync()
	{
		var milestones = await _httpClient.GetFromJsonAsync<List<DocumentationMilestoneDto>>("api/documentation");

		return milestones ?? new List<DocumentationMilestoneDto>();
	}

	public async Task<DocumentationMilestoneDto?> GetMilestoneByCategoryAsync(DocumentationCategory category)
	{
		return await _httpClient.GetFromJsonAsync<DocumentationMilestoneDto>($"api/documentation/{category}");
	}
	public async Task<DocumentationMilestoneDto?> UpdateMilestoneAsync(
	DocumentationCategory category,
	UpdateDocumentationMilestoneDto updateDto)
	{
		var response = await _httpClient.PutAsJsonAsync($"api/documentation/{category}", updateDto);

		if (!response.IsSuccessStatusCode)
		{
			return null;
		}

		return await response.Content.ReadFromJsonAsync<DocumentationMilestoneDto>();
	}
}