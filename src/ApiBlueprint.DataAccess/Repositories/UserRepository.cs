using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApiBlueprint.Core.Models.Entities;
using ApiBlueprint.DataAccess.Connection;
using ApiBlueprint.DataAccess.Contracts;
using ApiBlueprint.DataAccess.Extensions;

namespace ApiBlueprint.DataAccess.Repositories;

public sealed class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(DatabaseContext context)
        : base(context)
    {
    }

    public async Task<User> TryGet(Guid id, bool withTracking)
    {
        return await Context.Users
            .WithTracking(withTracking)
            .FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task<User> TryGet(string email, bool withTracking)
    {
        return await Context.Users
            .WithTracking(withTracking)
            .FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task<bool> IsEmailExists(string email)
    {
        return await Context.Users.AnyAsync(user => user.Email == email);
    }
}