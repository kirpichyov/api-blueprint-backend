﻿using ApiBlueprint.Core.Contracts;

namespace ApiBlueprint.Core.Common;

public sealed class HashingProvider : IHashingProvider
{
    public string GetHash(string value)
    {
        return BCrypt.Net.BCrypt.HashPassword(value);
    }

    public bool Verify(string value, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(value, hash);
    }
}