﻿using System;

namespace ApiBlueprint.Application.Models.Auth;

public sealed record RefreshTokenRequest
{
    public string AccessToken { get; set; }
    public Guid RefreshToken { get; set; }
}