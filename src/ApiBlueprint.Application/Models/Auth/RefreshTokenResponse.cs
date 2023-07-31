using System;

namespace ApiBlueprint.Application.Models.Auth;

public sealed record RefreshTokenResponse
{
    public string Token { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
}