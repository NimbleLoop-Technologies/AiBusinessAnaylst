using MongoDB.Bson;

namespace Nimbleloop.AiBusinessAnaylst.WebApp.Data;

public class Project
{
	public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

	public string Name { get; set; } = string.Empty;

	public string? Description { get; set; }

	public string? TechnicalDetails { get; set; }

	public string? ClickUpListId { get; set; }

	public string UserId { get; set; } = string.Empty;

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
