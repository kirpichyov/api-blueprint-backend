using ApiBlueprint.Application.Models.Endpoints;
using FluentValidation;

namespace ApiBlueprint.Application.Validators.Endpoints;

public sealed class UpdateEndpointGeneralInfoRequestValidator : AbstractValidator<UpdateEndpointGeneralInfoRequest>
{
    public UpdateEndpointGeneralInfoRequestValidator()
    {
        RuleFor(model => model.Title)
            .MaximumLength(130)
            .NotEmpty();

        RuleFor(model => model.Path)
            .MaximumLength(300)
            .NotEmpty();

        RuleFor(model => model.Method)
            .NotEmpty()
            .IsInEnum();
    }
}