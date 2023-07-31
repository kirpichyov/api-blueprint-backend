using System.Threading.Tasks;
using ApiBlueprint.Application.Models.Auth;

namespace ApiBlueprint.Application.Contracts;

public interface IAuthService
{
    Task<UserCreatedResponse> CreateUser(UserRegisterRequest request);
    Task<AuthResponse> GenerateJwtSession(SignInRequest request);
    Task<AuthResponse> RefreshJwtSession(RefreshTokenRequest request);
}