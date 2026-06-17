using System.ComponentModel.DataAnnotations;

namespace DomainModels.Dto;

public class CreateDeploymentNoteDto
{
	[Required]
	[MaxLength(100)]
	public string Title { get; set; } = string.Empty;
	[Required]
	public string Content { get; set; } = string.Empty;
	[Required]
	public string Category { get; set; } = string.Empty;
}