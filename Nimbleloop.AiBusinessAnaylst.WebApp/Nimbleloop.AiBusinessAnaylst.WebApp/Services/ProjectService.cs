using Microsoft.EntityFrameworkCore;
using Nimbleloop.AiBusinessAnaylst.WebApp.Data;
using Nimbleloop.AiBusinessAnaylst.WebApp.Models;

namespace Nimbleloop.AiBusinessAnaylst.WebApp.Services;

public class ProjectService(
	ApplicationDbContext dbContext,
	IHttpClientFactory httpClientFactory,
	ILogger<ProjectService> logger) : IProjectService
{
	public async Task<List<ProjectResponse>> GetProjectsAsync(string userId)
	{
		List<ProjectResponse> projects = await dbContext.Projects
			.Where(p => p.UserId == userId)
			.OrderByDescending(p => p.CreatedAt)
			.Select(x => new ProjectResponse
			{
				Id = x.Id,
				Name = x.Name,
				Description = x.Description,
				TechnicalDetails = x.TechnicalDetails,
				ClickUpListId = x.ClickUpListId,
				CreatedAt = x.CreatedAt,
				UpdatedAt = x.UpdatedAt
			})
			.ToListAsync();
		return projects;
	}

	public async Task<ProjectResponse> CreateProjectAsync(string userId, CreateProjectRequest request)
	{
		// Validate ClickUp List ID if provided
		if (!string.IsNullOrEmpty(request.ClickUpListId))
		{
			await ValidateClickUpListIdAsync(request.ClickUpListId);
		}

		Project project = new()
		{
			Name = request.Name,
			Description = request.Description,
			TechnicalDetails = request.TechnicalDetails,
			ClickUpListId = request.ClickUpListId,
			UserId = userId,
			CreatedAt = DateTime.UtcNow,
			UpdatedAt = DateTime.UtcNow
		};

		dbContext.Projects.Add(project);
		await dbContext.SaveChangesAsync();

		return MapToResponse(project);

		static ProjectResponse MapToResponse(Project project)
		{
			return new ProjectResponse
			{
				Id = project.Id,
				Name = project.Name,
				Description = project.Description,
				TechnicalDetails = project.TechnicalDetails,
				ClickUpListId = project.ClickUpListId,
				CreatedAt = project.CreatedAt,
				UpdatedAt = project.UpdatedAt
			};
		}

		async Task ValidateClickUpListIdAsync(string clickUpListId)
		{
			try
			{
				HttpClient client = httpClientFactory.CreateClient("ClickUp");
				HttpResponseMessage response = await client.GetAsync($"list/{clickUpListId}");

				if (!response.IsSuccessStatusCode)
				{
					throw new InvalidOperationException(
						$"The ClickUp List ID '{clickUpListId}' could not be validated. Please check the ID and try again.");
				}
			}
			catch (HttpRequestException ex)
			{
				logger.LogWarning(ex, "ClickUp API is unreachable. Skipping validation for List ID '{ClickUpListId}'.", clickUpListId);
			}
		}
	}
}
