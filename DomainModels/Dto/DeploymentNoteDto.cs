namespace DomainModels.Dto;

public class DeploymentNoteDto
{
	public int Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string Content { get; set; } = string.Empty;
	public string Category { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; }
}