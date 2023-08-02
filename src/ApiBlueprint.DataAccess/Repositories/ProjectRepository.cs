using System;
using System.Linq;
using System.Threading.Tasks;
using ApiBlueprint.Core.Models.Entities;
using ApiBlueprint.DataAccess.Connection;
using ApiBlueprint.DataAccess.Contracts;
using ApiBlueprint.DataAccess.Contracts.Includes;
using ApiBlueprint.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ApiBlueprint.DataAccess.Repositories;

public sealed class ProjectRepository : RepositoryBase<Project>, IProjectRepository
{
    public ProjectRepository(DatabaseContext context)
        : base(context)
    {
    }

    public Task<Project[]> GetAllForUser(Guid userId, bool withTracking)
    {
        return Context.Projects
            .WithTracking(withTracking)
            .Include(project => project.ProjectMembers)
            .Where(project => project.ProjectMembers.Any(member => member.UserId == userId))
            .OrderBy(project => project.CreatedAtUtc)
            .ToArrayAsync();
    }

    public Task<Project> TryGet(Guid id, bool withTracking, ProjectIncludes includes)
    {
        var queryBase = Context.Projects
            .WithTracking(withTracking)
            .Include(project => project.ProjectMembers)
            .Include(project => project.ProjectFolders)
            .AsQueryable();

        if (includes.HasFlag(ProjectIncludes.Endpoints))
        {
            queryBase = queryBase
                .Include(project => project.ProjectFolders)
                .ThenInclude(folder => folder.Endpoints);
        }
        
        return queryBase.FirstOrDefaultAsync(project => project.Id == id);
    }

    public Task<ProjectFolder> TryGetFolder(Guid folderId, bool withTracking)
    {
        return Context.ProjectFolders
            .WithTracking(withTracking)
            .Include(folder => folder.Project)
                .ThenInclude(project => project.ProjectMembers)
            .Include(folder => folder.Endpoints)
            .FirstOrDefaultAsync(folder => folder.Id == folderId);
    }
}