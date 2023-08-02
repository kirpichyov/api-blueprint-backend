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
    }
}