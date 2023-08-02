using System;
using System.Text.Json;
using ApiBlueprint.Core.Models.Enums;
using ApiBlueprint.Core.Models.ValueObjects;

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
        RequestContractJson = JsonSerializer.Serialize(GetDefaultContract());
        ResponseContractJson = JsonSerializer.Serialize(GetDefaultContract(statusCode: 200));
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

    public string RequestContractJson { get; private set; }
    public string ResponseContractJson { get; private set; }

    public EndpointContract GetRequestContract() => JsonSerializer.Deserialize<EndpointContract>(RequestContractJson);
    public EndpointContract GetResponseContract() => JsonSerializer.Deserialize<EndpointContract>(ResponseContractJson);

    public void SetRequestContract(EndpointContract contract)
    {
        RequestContractJson = JsonSerializer.Serialize(contract);
        UpdatedAtUtc = DateTime.UtcNow;
    }
    
    public void SetResponseContract(EndpointContract contract)
    {
        ResponseContractJson = JsonSerializer.Serialize(contract);
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

    private static EndpointContract GetDefaultContract(int? statusCode = null)
    {
        var contract = new EndpointContract(Array.Empty<EndpointParameter>(), "{}", "application/json", statusCode);
        return contract;
    } 
}