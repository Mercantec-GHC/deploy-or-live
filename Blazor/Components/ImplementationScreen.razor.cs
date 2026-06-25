using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components;

namespace Blazor.Components;

public partial class ImplementationScreen
{
    [Parameter]
    public IReadOnlyList<ImplementationScreenItem> Items { get; set; } = Array.Empty<ImplementationScreenItem>();

    [Parameter]
    public string? CategoryDisplayName { get; set; }

    protected string AriaLabel => string.IsNullOrWhiteSpace(CategoryDisplayName)
        ? "Implementation screens"
        : $"Implementation screens for {CategoryDisplayName}";
}

public sealed record ImplementationScreenItem(
    string Title,
    string? Description,
    string? Snippet,
    string? CommandDescription,
	string? Command,
    string? ImageDescription,
	string? ImageUrl,
    string? ImageAlt);
