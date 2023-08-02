using System;
using System.Text.Json;
using ApiBlueprint.Core.Models.Enums;
using ApiBlueprint.Core.Models.ValueObjects;

namespace ApiBlueprint.Core.Models.Entities;

public sealed class Endpoint : EntityBase<Guid>
{
    private EndpointParameter[] _requestParameters;
    private EndpointParameter[] _responseParameters;
    
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
        RequestParametersJson = "[]";
        ResponseParametersJson = "[]";
    }

    public Endpoint()
    {
    }

    public string Title { get; private set; }
    public string Path { get; private set; }
    public EndpointMethod Method { get; private set; }
    public Guid ProjectFolderId { get; }
    public ProjectFolder ProjectFolder { get; }
    
    public DateTime CreatedAtUtc { get; }
    public DateTime UpdatedAtUtc { get; private set; }
    
    public string RequestParametersJson { get; private set; }
    public string ResponseParametersJson { get; private set; }

    public EndpointParameter[] RequestParameters =>
        _requestParameters ??= JsonSerializer.Deserialize<EndpointParameter[]>(RequestParametersJson);

    public EndpointParameter[] ResponseParameters =>
        _responseParameters ??= JsonSerializer.Deserialize<EndpointParameter[]>(ResponseParametersJson);

    public void SetRequestParameters(EndpointParameter[] parameters)
    {
        RequestParametersJson = JsonSerializer.Serialize(parameters);
        _requestParameters = parameters;
        UpdatedAtUtc = DateTime.UtcNow;
    }
    
    public void SetResponseParameters(EndpointParameter[] parameters)
    {
        ResponseParametersJson = JsonSerializer.Serialize(parameters);
        _responseParameters = parameters;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void SetTitle(string title)
    {
        Title = title;
        UpdatedAtUtc = DateTime.UtcNow;
    }
    
    public void SetPath(string path)
    {
        Path = path;
        UpdatedAtUtc = DateTime.UtcNow;
    }
    
    public void SetMethod(EndpointMethod method)
    {
        Method = method;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}