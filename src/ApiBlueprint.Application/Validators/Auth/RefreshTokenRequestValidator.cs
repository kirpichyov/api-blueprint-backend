using FluentValidation;
using ApiBlueprint.Application.Models.Auth;

namespace ApiBlueprint.Application.Validators.Auth;

public sealed class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(request => request.AccessToken).NotEmpty();
        RuleFor(request => request.RefreshToken).NotEmpty();
    }
}
