using Blazor.Components;
using Blazor.Interfaces;
using DomainModels.Dto;
using DomainModels.Models;
using Microsoft.AspNetCore.Components;

namespace Blazor.Pages;

public partial class Home
{
    [Inject]
    private IHealthApiClient HealthApiClient { get; set; } = default!;

    [Inject]
    private IDocumentationApiClient DocumentationApiClient { get; set; } = default!;

    private List<DocumentationMilestoneDto> milestones = new();
    private string? errorMessage;
    private HealthStatusDto? healthStatus;
    private DocumentationMilestoneDto? editingMilestone;
    private UpdateDocumentationMilestoneDto editModel = new();
    private bool isEditMode = false;
    private string editApiKey = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await LoadHealthStatusAsync();
        await LoadMilestonesAsync();
    }

    private async Task LoadHealthStatusAsync()
    {
        try
        {
            healthStatus = await HealthApiClient.GetHealthAsync();
        }
        catch
        {
            healthStatus = new HealthStatusDto
            {
                Status = "Unreachable",
                Database = "Unknown",
                Environment = "Unknown",
                Timestamp = DateTime.UtcNow
            };
        }
    }

    private async Task LoadMilestonesAsync()
    {
        try
        {
            milestones = await DocumentationApiClient.GetMilestonesAsync();
        }
        catch (Exception ex)
        {
            errorMessage = $"Could not load documentation milestones. Is the API running? Error: {ex.Message}";
        }
    }

    private int CountCharacters(DocumentationMilestoneDto milestone)
    {
        if (milestone is null || string.IsNullOrWhiteSpace(milestone.DocumentationContent))
        {
            return 0;
        }

        var characterCount = milestone.DocumentationContent.Length;
        return characterCount;
    }

    private int TotalCharacterCount()
    {
        return milestones.Sum(m => CountCharacters(m));
    }

    private void StartEditingMilestone(DocumentationMilestoneDto milestone)
    {
        editingMilestone = milestone;

        editModel = new UpdateDocumentationMilestoneDto
        {
            CategoryDisplayName = milestone.CategoryDisplayName,
            DocumentationContent = milestone.DocumentationContent
        };
    }

    private void CancelEditingMilestone()
    {
        editingMilestone = null;
        editModel = new UpdateDocumentationMilestoneDto();
    }

    private async Task SaveMilestoneAsync()
    {
        if (editingMilestone is null)
        {
            return;
        }

        var updatedMilestone = await DocumentationApiClient.UpdateMilestoneAsync(
            editingMilestone.Category,
            editModel,
            editApiKey);

        if (updatedMilestone is null)
        {
            errorMessage = "Could not update documentation milestone. Check your edit key.";
            return;
        }

        var index = milestones.FindIndex(milestone => milestone.Category == updatedMilestone.Category);

        if (index >= 0)
        {
            milestones[index] = updatedMilestone;
        }

        editingMilestone = null;
        editModel = new UpdateDocumentationMilestoneDto();
    }

    private static string GetThemeClass(int index)
    {
        var themes = new[] { "theme-blue", "theme-violet", "theme-teal" };
        return themes[index % themes.Length];
    }

    private static IReadOnlyList<ImplementationScreenItem> GetImplementationScreensFor(DocumentationCategoryEnum category)
    {
        return category switch
        {
            DocumentationCategoryEnum.ProjectDescription => new[]
            {
                new ImplementationScreenItem(
                    "System architecture",
                    null,
					"""
                    Browser / User
                         |
                         v
                    Cloudflare
                    - DNS for victoria.mercantec.tech
                    - Public HTTPS endpoint
                    - Cloudflare Tunnel entry point
                         |
                         v
                    cloudflared tunnel connector on Ubuntu VM
                    - creates an outbound tunnel from my VM to Cloudflare
                    - avoids exposing the VM directly with normal public inbound traffic
                         |
                         v
                    VM: Ubuntu Server
                         |
                         v
                    Nginx reverse proxy
                         |
                         +--> Blazor WebAssembly container
                         |
                         +--> ASP.NET Core Web API container
                                      |
                                      v
                              SQL Server container

                    Deployment flow:
                    GitHub repository -> Dokploy -> Docker Compose -> VM containers
                    """,
                    null,
                    null,
                    null)
            },

            DocumentationCategoryEnum.InfrastructureAndDeployment => new[]
            {
                new ImplementationScreenItem(
                    "Docker Compose status",
                    "Terminal command used to verify the production containers on the VM.",
                    null,
                    "docker compose ps",
                    null,
                    null)
            },
            DocumentationCategoryEnum.CiCdPipeline => new[]
            {
                new ImplementationScreenItem(
                    "Dokploy deployment logs",
                    "Add a screenshot by placing it in Blazor/wwwroot/images/implementation-screens/ and setting ImageUrl to its relative path.",
                    null,
                    null,
                    null,
                    null)
            },
            _ => Array.Empty<ImplementationScreenItem>()
        };
    }
}
