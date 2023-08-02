using ApiBlueprint.Application.Models.Projects;
using FluentValidation;

namespace ApiBlueprint.Application.Validators.Projects;

public sealed class UpdateFolderRequestValidator : AbstractValidator<UpdateFolderRequest>
{
    public UpdateFolderRequestValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty()
            .MaximumLength(130);
    }
}