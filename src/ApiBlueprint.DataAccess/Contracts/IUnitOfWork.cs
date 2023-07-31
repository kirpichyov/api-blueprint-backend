using System;
using System.Threading.Tasks;

namespace ApiBlueprint.DataAccess.Contracts;

public interface IUnitOfWork
{
    IRefreshTokenRepository RefreshTokens { get; }
    IUserRepository Users { get; }
    IProjectRepository Projects { get; }

    public Task CommitTransactionAsync(Action action);
    public Task CommitTransactionAsync(Func<Task> action);
    public Task<TResult> CommitTransactionAsync<TResult>(Func<TResult> action);
    public Task CommitAsync();
}