using System;
using ApiBlueprint.Core.Models.Enums;

namespace ApiBlueprint.Core.Models.Entities;

public sealed class Endpoint : EntityBase<Guid>
{
    public Endpoint(
        string title,
        string path,
        EndpointMethod method,
        ProjectFolder projectFolder)
    {
        Title = title;
        Path = path;
        Method = method;
        ProjectFolderId = projectFolder.Id;
        ProjectFolder = projectFolder;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public Endpoint()
    {
    }

    public string Title { get; } 
    public string Path { get; } 
    public EndpointMethod Method { get; }
    public Guid ProjectFolderId { get; }
    public ProjectFolder ProjectFolder { get; }
    
    public DateTime CreatedAtUtc { get; }
    public DateTime UpdatedAtUtc { get; }
}