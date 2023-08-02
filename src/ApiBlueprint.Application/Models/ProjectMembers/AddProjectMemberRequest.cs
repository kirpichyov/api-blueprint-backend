namespace ApiBlueprint.Application.Models.ProjectMembers;

public sealed record AddProjectMemberRequest
{
    public string UserEmail { get; init; }
    public ProjectMemberRoleModel? Role { get; init; }
}