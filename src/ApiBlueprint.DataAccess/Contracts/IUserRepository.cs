using System;
using System.Threading.Tasks;
using ApiBlueprint.Core.Models.Entities;

namespace ApiBlueprint.DataAccess.Contracts;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<User> TryGet(Guid id, bool withTracking);
    Task<User> TryGet(string email, bool withTracking);
    Task<bool> IsEmailExists(string email);
}