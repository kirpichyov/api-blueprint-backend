﻿using System;
using System.Threading.Tasks;
using ApiBlueprint.Core.Models.Entities;
using ApiBlueprint.DataAccess.Contracts.Includes;

namespace ApiBlueprint.DataAccess.Contracts;

public interface IProjectRepository : IRepositoryBase<Project>
{
    Task<Project[]> GetAllForUser(Guid userId, bool withTracking);
    Task<Project> TryGet(Guid id, bool withTracking, ProjectIncludes includes = ProjectIncludes.Default);
    Task<ProjectFolder> TryGetFolder(Guid folderId, bool withTracking);
}