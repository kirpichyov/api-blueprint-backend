using NetEscapades.EnumGenerators;

namespace ApiBlueprint.Core.Models.Enums;

[EnumExtensions]
public enum ProjectMemberRole
{
    Owner,
    Admin,
    Viewer
}