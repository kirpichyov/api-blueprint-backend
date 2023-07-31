using System;
using ApiBlueprint.Core.Models.Entities;
using ApiBlueprint.Core.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiBlueprint.DataAccess.EntityConfigurations;

public sealed class ProjectMemberConfiguration : EntityConfigurationBase<ProjectMember, Guid>
{
    public override void Configure(EntityTypeBuilder<ProjectMember> builder)
    {
        base.Configure(builder);

        builder.Property(member => member.Role)
            .HasConversion(@enum => @enum.ToStringFast(), @string => ParseRole(@string))
            .IsRequired();

        builder.HasOne(member => member.User)
            .WithMany()
            .HasForeignKey(member => member.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }

    private static ProjectMemberRole ParseRole(string @string)
    {
        if (ProjectMemberRoleExtensions.TryParse(@string, out var @enum, ignoreCase: true))
        {
            return @enum;
        }

        throw new ArgumentException($"Value '{@string}' is not parsable to {nameof(ProjectMemberRole)}.");
    }
}