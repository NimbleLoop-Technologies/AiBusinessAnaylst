namespace Nimbleloop.AiBusinessAnaylst.WebApp.Models;

public class CreateProjectRequest
{
	public string Name { get; set; } = string.Empty;

	public string? Description { get; set; }

	public string? TechnicalDetails { get; set; }

	public string? ClickUpListId { get; set; }
}
