using ApiBlueprint.Application.Models.Endpoints;
using FluentValidation;

namespace ApiBlueprint.Application.Validators.Endpoints;

public sealed class UpdateEndpointContractRequestValidator : AbstractValidator<UpdateEndpointContractRequest>
{
    public UpdateEndpointContractRequestValidator()
    {
        RuleForEach(model => model.Parameters)
            .NotNull()
            .SetValidator(new UpdateEndpointParameterModelValidator());

        RuleFor(model => model.ContentType)
            .NotEmpty();
        
        RuleFor(model => model.StatusCode)
            .GreaterThanOrEqualTo(100)
            .LessThanOrEqualTo(599)
            .When(model => model.StatusCode.HasValue);

        RuleFor(model => model.ContentJson)
            .NotEmpty();
    }
}