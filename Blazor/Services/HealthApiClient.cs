using System.Net.Http.Json;
using Blazor.Interfaces;
using DomainModels.Dto;

namespace Blazor.Services;

public class HealthApiClient : IHealthApiClient
{
	private readonly HttpClient _httpClient;

	public HealthApiClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<HealthStatusDto?> GetHealthAsync()
	{
		return await _httpClient.GetFromJsonAsync<HealthStatusDto>("api/health");
	}
}