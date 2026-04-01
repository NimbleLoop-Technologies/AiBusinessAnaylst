using Nimbleloop.AiBusinessAnaylst.WebApp.Models;

namespace Nimbleloop.AiBusinessAnaylst.WebApp.Services;

public interface IProjectService
{
	Task<List<ProjectResponse>> GetProjectsAsync(string userId);

	Task<ProjectResponse> CreateProjectAsync(string userId, CreateProjectRequest request);
}
