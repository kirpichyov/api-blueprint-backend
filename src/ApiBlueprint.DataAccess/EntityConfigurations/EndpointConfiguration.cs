using System;
using ApiBlueprint.Core.Models.Entities;
using ApiBlueprint.Core.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiBlueprint.DataAccess.EntityConfigurations;

public sealed class EndpointConfiguration : EntityConfigurationBase<Endpoint, Guid>
{
    public override void Configure(EntityTypeBuilder<Endpoint> builder)
    {
        base.Configure(builder);

        builder.Property(endpoint => endpoint.Title).IsRequired();
        builder.Property(endpoint => endpoint.Path).IsRequired();
        builder.Property(endpoint => endpoint.CreatedAtUtc).IsRequired();
        builder.Property(endpoint => endpoint.UpdatedAtUtc).IsRequired();
        builder.Property(endpoint => endpoint.RequestParametersJson).IsRequired();
        builder.Property(endpoint => endpoint.ResponseParametersJson).IsRequired();

        builder.Ignore(endpoint => endpoint.RequestParameters);
        builder.Ignore(endpoint => endpoint.ResponseParameters);

        builder.Property(endpoint => endpoint.Method)
            .HasConversion(@enum => @enum.ToStringFast(), @string => ParseEndpointMethod(@string))
            .IsRequired();

        builder.HasOne(endpoint => endpoint.ProjectFolder)
            .WithMany(folder => folder.Endpoints)
            .HasForeignKey(endpoint => endpoint.ProjectFolderId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
    
    private static EndpointMethod ParseEndpointMethod(string @string)
    {
        if (EndpointMethodExtensions.TryParse(@string, out var @enum, ignoreCase: true))
        {
            return @enum;
        }

        throw new ArgumentException($"Value '{@string}' is not parsable to {nameof(EndpointMethod)}.");
    }
}