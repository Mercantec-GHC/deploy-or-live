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
                    "Full teck stach",
					null,
					"Frontend: Blazor WebAssembly (.NET 8)" +
                    "\r\nBackend: ASP.NET Core Web API (.NET 8)" +
                    "\r\nProgramming Language: C#" +
                    "\r\nShared Models: .NET class library / DomainModels project" +
                    "\r\nDatabase: Microsoft SQL Server 2022" +
                    "\r\nDatabase Access: Entity Framework Core" +
                    "\r\nFrontend Web Server: Nginx inside the Blazor container" +
                    "\r\nReverse Proxy/Routing: Cloudflare Tunnel, Dokploy/Traefik, and Nginx for internal /api routing" +
                    "\r\nContainerization: Docker, Docker Compose" +
                    "\r\nServer OS: Ubuntu Server 24.04 LTS" +
                    "\r\nDomain/DNS: Cloudflare DNS" +
                    "\r\nHTTPS/TLS: Cloudflare-managed HTTPS/TLS through Cloudflare Tunnel" +
                    "\r\nVersion Control: Git, GitHub" +
                    "\r\nDeployment: Dokploy with Docker Compose" +
                    "\r\nMonitoring: Docker logs, health checks, Dokploy monitoring" +
                    "\r\nSecurity: SSH, firewall, environment variables, edit key, security headers",					
                    null,
					null,
					null,
					null,
					null)
			},

            DocumentationCategoryEnum.InfrastructureAndDeployment => new[]
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
                    git push
                         |
                         v
                    GitHub repository
                         |
                         v
                    GitHub Actions / Dokploy trigger
                         |
                         v
                    Dokploy on Ubuntu VM
                         |
                         v
                    Docker Compose builds/recreates containers
                         |
                         v
                    Running application:
                    - Blazor container
                    - API container
                    - SQL Server container
                    """,
					null,
					null,
					"VM setup screenshot",
					"images/implementation-screens/Screenshot-VM-setup.png",
	                "VM setup screenshot"
					)
            },

            DocumentationCategoryEnum.CiCdPipeline => new[]
            {
                new ImplementationScreenItem(
                    "CI/CD flow diagram",
                    null,
                    """
                    Developer pushes to main
                         |
                         v
                    GitHub repository
                         |
                         v
                    GitHub Actions workflow starts
                         |
                         v
                    Self-hosted runner on VM executes job
                         |
                         v
                    Runner calls Dokploy on localhost:3000
                         |
                         v
                    Dokploy redeploys Docker Compose stack
                         |
                         v
                    Live website updates

                    Result:
                    - Docker Compose stack is rebuilt/recreated
                    - Blazor WebAssembly container is updated
                    - API container is updated
                    - Live site runs the newest version from main
                    """,
					"deploy.yml",
					"name: Deploy with Dokploy\r\n\r\non:\r\n  push:\r\n    branches:\r\n      - main\r\n\r\njobs:\r\n  deploy:\r\n    runs-on: self-hosted\r\n\r\n    steps:\r\n      - name: Trigger Dokploy deployment locally\r\n        shell: bash\r\n        run: |\r\n          set -e\r\n\r\n          echo \"Triggering Dokploy deployment...\"\r\n\r\n          RESPONSE=$(curl -sS -X POST \"${{ secrets.DOKPLOY_WEBHOOK_URL }}\" \\\r\n            -H \"Content-Type: application/json\" \\\r\n            -H \"X-GitHub-Event: push\" \\\r\n            -d '{\"ref\":\"refs/heads/main\",\"repository\":{\"full_name\":\"Mercantec-GHC/deploy-or-live\"}}')\r\n\r\n          echo \"Dokploy response:\"\r\n          echo \"$RESPONSE\"\r\n\r\n          echo \"$RESPONSE\" | grep -q \"Compose deployed successfully\"",
                    "A Dokploy deployments status screenshot",
					"images/implementation-screens/Screenshot-Dokploy-deployments.png",
					"A Dokploy deployment screenshot")
            },
            DocumentationCategoryEnum.Security => new[]
            {
                new ImplementationScreenItem(
                    "Security measures",
					null,
                    """
                    1. SSH access to the server is restricted to specific users and keys.
                    2. Firewall rules are configured to allow only necessary traffic (e.g., HTTP, HTTPS, SSH).
                    3. Environment variables are used for sensitive information (e.g., database connection strings, API keys).
                    4. An edit key is required to update documentation milestones via the API.
                    5. Security headers are configured in Nginx to enhance security:
                       - X-Frame-Options: DENY
                       - X-Content-Type-Options: nosniff
                       - Referrer-Policy: strict-origin-when-cross-origin
                       - Content-Security-Policy: default-src 'self'; script-src 'self' 'unsafe-inline' 'wasm-unsafe-eval'; style-src 'self' 'unsafe-inline'; img-src 'self' data:; connect-src 'self' https://victoria.mercantec.tech;
                    """,
					"nginx.conf",
					"server {\r\n    listen 80;\r\n\r\n    root /usr/share/nginx/html;\r\n    index index.html;\r\n\r\n    location /api {\r\n        proxy_pass http://api:8080;\r\n        proxy_http_version 1.1;\r\n        proxy_set_header Host $host;\r\n        proxy_set_header X-Real-IP $remote_addr;\r\n    }\r\n\r\n    location / {\r\n        try_files $uri $uri/ /index.html;\r\n\r\n        add_header X-Frame-Options \"DENY\" always;\r\n        add_header X-Content-Type-Options \"nosniff\" always;\r\n        add_header Referrer-Policy \"strict-origin-when-cross-origin\" always;\r\n        add_header Content-Security-Policy \"default-src 'self'; script-src 'self' 'unsafe-inline' 'wasm-unsafe-eval'; style-src 'self' 'unsafe-inline'; img-src 'self' data:; connect-src 'self' https://victoria.mercantec.tech;\" always;\r\n    }\r\n}",
                    "A screenshot of Dev tools with security headers",
                    "images/implementation-screens/Screenshot-Nginx-configuration.png",
                    "A screenshot of the Nginx configuration")
                },
			_ => Array.Empty<ImplementationScreenItem>()
        };
    }
}
