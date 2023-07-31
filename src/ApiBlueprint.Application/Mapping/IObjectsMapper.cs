using System;
using System.Collections.Generic;
using ApiBlueprint.Application.Models.Profile;
using ApiBlueprint.Application.Models.Projects;
using ApiBlueprint.Core.Models.Entities;

namespace ApiBlueprint.Application.Mapping;

public interface IObjectsMapper
{
    CurrentUserProfileResponse ToCurrentUserProfileResponse(User user);
    ProjectSummaryResponse ToProjectSummaryResponse(Project project);

    IReadOnlyCollection<TDestination> MapCollection<TSource, TDestination>(
        IEnumerable<TSource> sources,
        Func<TSource, TDestination> rule);
}