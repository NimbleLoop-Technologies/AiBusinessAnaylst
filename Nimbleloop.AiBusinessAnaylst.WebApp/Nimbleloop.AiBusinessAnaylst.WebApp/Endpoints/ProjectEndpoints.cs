using System.Security.Claims;
using FluentValidation;
using Nimbleloop.AiBusinessAnaylst.WebApp.Models;
using Nimbleloop.AiBusinessAnaylst.WebApp.Services;

namespace Nimbleloop.AiBusinessAnaylst.WebApp.Endpoints;

public static class ProjectEndpoints
{
	public static IEndpointRouteBuilder MapProjectEndpoints(this IEndpointRouteBuilder endpoints)
	{
		var group = endpoints.MapGroup("/api/projects").RequireAuthorization();

		group.MapGet("/", async (ClaimsPrincipal user, IProjectService projectService) =>
		{
			var userId = GetUserId(user);
			if (userId is null)
			{
				return Results.Unauthorized();
			}

			var projects = await projectService.GetProjectsAsync(userId);
			return Results.Ok(projects);
		});

		group.MapPost("/", async (
			ClaimsPrincipal user,
			CreateProjectRequest request,
			IValidator<CreateProjectRequest> validator,
			IProjectService projectService) =>
		{
			var userId = GetUserId(user);
			if (userId is null)
			{
				return Results.Unauthorized();
			}

			var validationResult = await validator.ValidateAsync(request);
			if (!validationResult.IsValid)
			{
				return Results.ValidationProblem(validationResult.ToDictionary());
			}

			try
			{
				var project = await projectService.CreateProjectAsync(userId, request);
				return Results.Created($"/api/projects/{project.Id}", project);
			}
			catch (InvalidOperationException ex)
			{
				return Results.BadRequest(new { error = ex.Message });
			}
		});

		return endpoints;
	}

	private static string? GetUserId(ClaimsPrincipal user)
	{
		return user.FindFirstValue(ClaimTypes.NameIdentifier);
	}
}
