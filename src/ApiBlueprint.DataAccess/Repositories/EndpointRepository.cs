using System;
using System.Linq;
using System.Threading.Tasks;
using ApiBlueprint.Core.Models.Entities;
using ApiBlueprint.DataAccess.Connection;
using ApiBlueprint.DataAccess.Contracts;
using ApiBlueprint.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ApiBlueprint.DataAccess.Repositories;

public sealed class EndpointRepository : RepositoryBase<Endpoint>, IEndpointRepository
{
    public EndpointRepository(DatabaseContext context)
        : base(context)
    {
    }

    public Task<Endpoint> TryGet(Guid endpointId, bool withTracking)
    {
        return Context.Endpoints
            .WithTracking(withTracking)
            .Include(endpoint => endpoint.ProjectFolder)
                .ThenInclude(endpoint => endpoint.Project)
                .ThenInclude(project => project.ProjectMembers)
            .OrderBy(endpoint => endpoint.CreatedAtUtc)
            .FirstOrDefaultAsync(endpoint => endpoint.Id == endpointId);
    }
}