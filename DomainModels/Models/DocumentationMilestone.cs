namespace DomainModels.Models;

public class DocumentationMilestone
{
	public int Id { get; set; }
	public DocumentationCategory Category { get; set; }
	public string WhatIDid { get; set; } = string.Empty;
	public string CommandsUsed { get; set; } = string.Empty;
	public string ScreenshotNotes { get; set; } = string.Empty;
	public string ProblemsFaced { get; set; } = string.Empty;
	public string WhatILearned { get; set; } = string.Empty;
	public string ProductionConnection { get; set; } = string.Empty;
	public DateTime UpdatedAt { get; set; }
}