using ApiBlueprint.Application.Models.Projects;
using FluentValidation;

namespace ApiBlueprint.Application.Validators.Projects;

public sealed class UpdateProjectRequestValidator : AbstractValidator<UpdateProjectRequest>
{
    public UpdateProjectRequestValidator()
    {
        RuleFor(model => model.Name).NotEmpty();

        RuleFor(model => model.Description)
            .NotEmpty()
            .When(model => model.Description is not null);
    }
}