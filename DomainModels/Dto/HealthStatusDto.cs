namespace DomainModels.Dto;

public class HealthStatusDto
{
	public string Status { get; set; } = string.Empty;

	public string Database { get; set; } = string.Empty;

	public string Environment { get; set; } = string.Empty;

	public DateTime Timestamp { get; set; }
}