using DomainModels.Dto;

namespace Blazor.Interfaces;

public interface INotesApiClient
{
	Task<List<DeploymentNoteDto>> GetNotesAsync();

	Task<DeploymentNoteDto?> CreateNoteAsync(CreateDeploymentNoteDto note);

	Task DeleteNoteAsync(int id);
}