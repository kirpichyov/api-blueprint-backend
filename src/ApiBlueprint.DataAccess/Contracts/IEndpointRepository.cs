using System;
using System.Threading.Tasks;
using ApiBlueprint.Core.Models.Entities;

namespace ApiBlueprint.DataAccess.Contracts;

public interface IEndpointRepository : IRepositoryBase<Endpoint>
{
    Task<Endpoint> TryGet(Guid endpointId, bool withTracking);
}