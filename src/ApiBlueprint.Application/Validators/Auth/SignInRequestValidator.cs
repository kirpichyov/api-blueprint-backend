using FluentValidation;
using ApiBlueprint.Application.Models.Auth;

namespace ApiBlueprint.Application.Validators.Auth;

public sealed class SignInRequestValidator : AbstractValidator<SignInRequest>
{
    public SignInRequestValidator()
    {
        RuleFor(model => model.Email).NotEmpty();
        RuleFor(model => model.Password).NotEmpty();
    }
}