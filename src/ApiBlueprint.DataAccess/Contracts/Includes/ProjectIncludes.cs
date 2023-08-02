using System;

namespace ApiBlueprint.DataAccess.Contracts.Includes;

[Flags]
public enum ProjectIncludes
{
    Default,
    Endpoints,
    MembersUser,
}