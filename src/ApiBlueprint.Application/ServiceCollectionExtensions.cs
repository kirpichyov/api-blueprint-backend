using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ApiBlueprint.Application.Contracts;
using ApiBlueprint.Application.Mapping;
using ApiBlueprint.Application.Services;
using ApiBlueprint.Application.Validators.Auth;
using ApiBlueprint.Core.Common;
using ApiBlueprint.Core.Contracts;
using ApiBlueprint.Core.Options;

namespace ApiBlueprint.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IObjectsMapper, ObjectsMapper>();
        services.AddScoped<IHashingProvider, HashingProvider>();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IProjectsService, ProjectsService>();

        services.AddScoped<IAuthValidatorsAggregate, AuthValidatorsAggregate>();

        services.Configure<ImageGenerationOptions>(configuration.GetSection(nameof(ImageGenerationOptions)));

        return services;
    }
}