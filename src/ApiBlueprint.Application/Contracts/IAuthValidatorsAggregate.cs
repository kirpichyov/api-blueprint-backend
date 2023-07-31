using FluentValidation;
using ApiBlueprint.Application.Models.Auth;

namespace ApiBlueprint.Application.Contracts;

public interface IAuthValidatorsAggregate
{
    public IValidator<SignInRequest> SignInValidator { get; }
    public IValidator<RefreshTokenRequest> RefreshTokenValidator { get; }
    public IValidator<UserRegisterRequest> UserRegisterValidator { get; }
}