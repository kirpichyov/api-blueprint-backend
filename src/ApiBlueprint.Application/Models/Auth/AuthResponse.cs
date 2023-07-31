using System;

namespace ApiBlueprint.Application.Models.Auth;

public sealed record AuthResponse
{
    public JwtResponse Jwt { get; set; }
    public RefreshTokenResponse RefreshToken { get; set; }
    public Guid UserId { get; set; }
}