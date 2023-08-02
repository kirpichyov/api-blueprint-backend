using ApiBlueprint.Application.Models.ProjectMembers;
using FluentValidation;

namespace ApiBlueprint.Application.Validators.ProjectMembers;

public sealed class AddProjectMemberRequestValidator : AbstractValidator<AddProjectMemberRequest>
{
    public AddProjectMemberRequestValidator()
    {
        RuleFor(model => model.UserEmail).NotEmpty();
        RuleFor(model => model.Role).NotEmpty();
    }
}