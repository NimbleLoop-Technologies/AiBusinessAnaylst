using FluentValidation;
using Nimbleloop.AiBusinessAnaylst.WebApp.Models;

namespace Nimbleloop.AiBusinessAnaylst.WebApp.Validators;

public class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequest>
{
	public CreateProjectRequestValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Project name is required.")
			.MaximumLength(200).WithMessage("Project name must not exceed 200 characters.");

		RuleFor(x => x.Description)
			.MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.");

		RuleFor(x => x.TechnicalDetails)
			.MaximumLength(5000).WithMessage("Technical details must not exceed 5000 characters.");

		RuleFor(x => x.ClickUpListId)
			.Matches(@"^[a-zA-Z0-9\-]+$")
			.When(x => !string.IsNullOrEmpty(x.ClickUpListId))
			.WithMessage("ClickUp List ID must contain only alphanumeric characters and hyphens.");
	}
}
