using ApiBlueprint.Application.Models.Endpoints;
using FluentValidation;

namespace ApiBlueprint.Application.Validators.Endpoints;

public sealed class UpdateEndpointParameterModelValidator : AbstractValidator<UpdateEndpointParameterModel>
{
    public UpdateEndpointParameterModelValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty()
            .MaximumLength(130);
        
        RuleFor(model => model.DataType)
            .NotEmpty()
            .MaximumLength(130);

        RuleFor(model => model.Notes)
            .NotEmpty()
            .MaximumLength(256);
        
        RuleFor(model => model.In)
            .NotEmpty()
            .IsInEnum();
    }
}