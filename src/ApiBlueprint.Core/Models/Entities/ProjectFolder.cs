using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace ApiBlueprint.Core.Models.Entities;

public sealed class ProjectFolder : EntityBase<Guid>
{
    private readonly List<Endpoint> _endpoints = new();

    public ProjectFolder(string name, Project project)
    {
        Name = name;
        ProjectId = project.Id;
        Project = project;
        CreatedAtUtc = DateTime.UtcNow;
    }

    private ProjectFolder()
    {
    }

    public string Name { get; private set; }
    public Guid ProjectId { get; }
    public Project Project { get; }
    public DateTime CreatedAtUtc { get; }
    public IReadOnlyCollection<Endpoint> Endpoints => _endpoints;

    public void SetName(string name)
    {
        Name = name;
    }
    
    public Result AddEndpoint(Endpoint endpoint)
    {
        if (endpoint.ProjectFolder != this)
        {
            return Result.Failure("Endpoint is required to be linked to the target folder.");
        }

        _endpoints.Add(endpoint);
        return Result.Success();
    }
}