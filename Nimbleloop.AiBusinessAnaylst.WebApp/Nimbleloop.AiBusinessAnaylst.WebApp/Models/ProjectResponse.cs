namespace Nimbleloop.AiBusinessAnaylst.WebApp.Models;

public class ProjectResponse
{
	public string Id { get; set; } = string.Empty;

	public string Name { get; set; } = string.Empty;

	public string? Description { get; set; }

	public string? TechnicalDetails { get; set; }

	public string? ClickUpListId { get; set; }

	public DateTime CreatedAt { get; set; }

	public DateTime UpdatedAt { get; set; }
}
