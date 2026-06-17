using DomainModels.Dto;

namespace Blazor.Interfaces;

public interface IHealthApiClient
{
	Task<HealthStatusDto?> GetHealthAsync();
}