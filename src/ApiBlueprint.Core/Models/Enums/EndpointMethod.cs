using NetEscapades.EnumGenerators;

namespace ApiBlueprint.Core.Models.Enums;

[EnumExtensions]
public enum EndpointMethod
{
    Get,
    Post,
    Put,
    Patch,
    Delete,
    Options,
}