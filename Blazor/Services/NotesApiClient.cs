using System.Net.Http.Json;
using Blazor.Interfaces;
using DomainModels.Dto;

namespace Blazor.Services;

public class NotesApiClient : INotesApiClient
{
	private readonly HttpClient _httpClient;

	public NotesApiClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<List<DeploymentNoteDto>> GetNotesAsync()
	{
		var notes = await _httpClient.GetFromJsonAsync<List<DeploymentNoteDto>>("api/notes");

		return notes ?? new List<DeploymentNoteDto>();
	}

	public async Task<DeploymentNoteDto?> CreateNoteAsync(CreateDeploymentNoteDto note)
	{
		var response = await _httpClient.PostAsJsonAsync("api/notes", note);

		if (!response.IsSuccessStatusCode)
		{
			return null;
		}

		return await response.Content.ReadFromJsonAsync<DeploymentNoteDto>();
	}

	public async Task DeleteNoteAsync(int id)
	{
		var response = await _httpClient.DeleteAsync($"api/notes/{id}");

		response.EnsureSuccessStatusCode();
	}
}