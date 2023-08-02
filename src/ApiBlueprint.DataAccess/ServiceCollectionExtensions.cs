using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ApiBlueprint.DataAccess.Connection;
using ApiBlueprint.DataAccess.Contracts;
using ApiBlueprint.DataAccess.Repositories;

namespace ApiBlueprint.DataAccess;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccessServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(nameof(DatabaseContext)));

                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

                if (environment.IsDevelopment())
                {
                    options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
                    options.EnableSensitiveDataLogging();
                }
            }
        );

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokensRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IEndpointRepository, EndpointRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}