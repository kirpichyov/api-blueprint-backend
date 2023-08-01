using ApiBlueprint.Application.Models.Projects;
using FluentValidation;

namespace ApiBlueprint.Application.Validators.Projects;

public sealed class CreateFolderRequestValidator : AbstractValidator<CreateFolderRequest>
{
    public CreateFolderRequestValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty()
            .MaximumLength(130);
    }
}