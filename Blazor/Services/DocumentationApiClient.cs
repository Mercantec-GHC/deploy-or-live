using System.Net.Http.Json;
using Blazor.Interfaces;
using DomainModels.Dto;
using DomainModels.Models;

namespace Blazor.Services;

public class DocumentationApiClient : IDocumentationApiClient
{
	private readonly HttpClient _httpClient;
	private const string EditApiKeyHeaderName = "X-Documentation-Edit-Key";

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
  UpdateDocumentationMilestoneDto updateDto,
	string editApiKey)
	{
        using var request = new HttpRequestMessage(HttpMethod.Put, $"api/documentation/{category}")
		{
			Content = JsonContent.Create(updateDto)
		};

		request.Headers.Add(EditApiKeyHeaderName, editApiKey);

		var response = await _httpClient.SendAsync(request);

		if (!response.IsSuccessStatusCode)
		{
			return null;
		}

		return await response.Content.ReadFromJsonAsync<DocumentationMilestoneDto>();
	}
}