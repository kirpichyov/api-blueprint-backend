using System;
using System.Collections.Generic;
using System.Linq;
using ApiBlueprint.Application.Models.Endpoints;
using ApiBlueprint.Application.Models.Profile;
using ApiBlueprint.Application.Models.Projects;
using ApiBlueprint.Core.Models.Entities;

namespace ApiBlueprint.Application.Mapping;

public sealed class ObjectsMapper : IObjectsMapper
{
    public CurrentUserProfileResponse ToCurrentUserProfileResponse(User user)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        return new CurrentUserProfileResponse
        {
            Id = user.Id,
            Email = user.Email,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Fullname = user.Fullname,
        };
    }

    public ProjectSummaryResponse ToProjectSummaryResponse(Project project)
    {
        ArgumentNullException.ThrowIfNull(project, nameof(project));

        return new ProjectSummaryResponse()
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            ImageUrl = project.ImageUrl,
            CreatedAtUtc = project.CreatedAtUtc,
            UpdatedAtUtc = project.UpdatedAtUtc,
            MembersCount = project.ProjectMembers.Count,
        };
    }

    public FolderResponse ToFolderResponse(ProjectFolder folder)
    {
        ArgumentNullException.ThrowIfNull(folder, nameof(folder));

        return new FolderResponse()
        {
            Id = folder.Id,
            Name = folder.Name,
            CreatedAtUtc = folder.CreatedAtUtc,
        };
    }

    public EndpointResponse ToEndpointResponse(Endpoint endpoint)
    {
        ArgumentNullException.ThrowIfNull(endpoint, nameof(endpoint));

        return new EndpointResponse()
        {
            Id = endpoint.Id,
            Method = endpoint.Method,
            Path = endpoint.Path,
            Title = endpoint.Title,
        };
    }

    public EndpointSummaryResponse ToEndpointSummaryResponse(Endpoint endpoint)
    {
        ArgumentNullException.ThrowIfNull(endpoint, nameof(endpoint));

        return new EndpointSummaryResponse()
        {
            Id = endpoint.Id,
            Method = endpoint.Method,
            Path = endpoint.Path,
            Title = endpoint.Title,
        };
    }

    public IReadOnlyCollection<TDestination> MapCollection<TSource, TDestination>(
        IEnumerable<TSource> sources,
        Func<TSource, TDestination> rule)
    {
        return sources?.Select(rule).ToArray();
    }
}