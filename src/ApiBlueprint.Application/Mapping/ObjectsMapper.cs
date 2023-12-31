﻿using System;
using System.Collections.Generic;
using System.Linq;
using ApiBlueprint.Application.Models;
using ApiBlueprint.Application.Models.Endpoints;
using ApiBlueprint.Application.Models.Profile;
using ApiBlueprint.Application.Models.ProjectMembers;
using ApiBlueprint.Application.Models.Projects;
using ApiBlueprint.Core.Models.Entities;
using ApiBlueprint.Core.Models.ValueObjects;

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

    public ProjectSummaryResponse ToProjectSummaryResponse(Project project, Guid currentUserId)
    {
        ArgumentNullException.ThrowIfNull(project, nameof(project));

        ProjectAccessInfoResponse MapAccessInfo()
        {
            var member = project.ProjectMembers.First(member => member.UserId == currentUserId);

            return new ProjectAccessInfoResponse()
            {
                UserId = member.UserId,
                MemberId = member.Id,
                Role = member.Role,
                HasAccess = true,
                CanEdit = project.CanEdit(member.UserId),
            };
        }
        
        return new ProjectSummaryResponse()
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            ImageUrl = project.ImageUrl,
            CreatedAtUtc = project.CreatedAtUtc,
            UpdatedAtUtc = project.UpdatedAtUtc,
            MembersCount = project.ProjectMembers.Count,
            AccessInfo = MapAccessInfo(),
            Members = project.ProjectMembers.Select(ToProjectMemberResponse).ToArray(),
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
            Endpoints = folder?.Endpoints
                .OrderBy(endpoint => endpoint.CreatedAtUtc)
                .Select(endpoint => new FolderEndpointModel()
                {
                    Id = endpoint.Id,
                    FolderId = endpoint.ProjectFolderId,
                    Method = endpoint.Method,
                    Path = endpoint.Path,
                    Title = endpoint.Title,
                })
                .ToArray(),
        };
    }

    public EndpointResponse ToEndpointResponse(Endpoint endpoint)
    {
        ArgumentNullException.ThrowIfNull(endpoint, nameof(endpoint));

        var requestContract = endpoint.GetRequestContract();
        var responseContract = endpoint.GetResponseContract();
        
        return new EndpointResponse()
        {
            Id = endpoint.Id,
            FolderId = endpoint.ProjectFolderId,
            Method = endpoint.Method,
            Path = endpoint.Path,
            Title = endpoint.Title,
            Request = ToEndpointDataModel(requestContract),
            Response = ToEndpointDataModel(responseContract),
        };
    }

    public ProjectMemberResponse ToProjectMemberResponse(ProjectMember member)
    {
        ArgumentNullException.ThrowIfNull(member, nameof(member));
        ArgumentNullException.ThrowIfNull(member.User, nameof(member.User));

        return new ProjectMemberResponse()
        {
            MemberId = member.Id,
            Role = member.Role,
            User = new UserModel()
            {
                Id = member.UserId,
                Email = member.User.Email,
                Firstname = member.User.Firstname,
                Lastname = member.User.Lastname,
                Fullname = $"{member.User.Firstname} {member.User.Lastname}",
            },
        };
    }

    public IReadOnlyCollection<TDestination> MapCollection<TSource, TDestination>(
        IEnumerable<TSource> sources,
        Func<TSource, TDestination> rule)
    {
        return sources?.Select(rule).ToArray();
    }

    private static EndpointParameterModel ToEndpointParameterModel(EndpointParameter parameter)
    {
        ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));

        return new EndpointParameterModel()
        {
            Name = parameter.Name,
            Notes = parameter.Notes,
            In = parameter.In,
            DataType = parameter.DataType,
            CreatedAtUtc = parameter.CreatedAtUtc,
        };
    }

    private static EndpointDataModel ToEndpointDataModel(EndpointContract contract)
    {
        ArgumentNullException.ThrowIfNull(contract, nameof(contract));

        return new EndpointDataModel()
        {
            Parameters = contract.Parameters
                .Select(ToEndpointParameterModel)
                .ToArray(),
            ContentType = contract.ContentType,
            StatusCode = contract.StatusCode,
            ContentJson = contract.Body,
        };
    }
}