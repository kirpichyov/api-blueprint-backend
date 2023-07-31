using System;
using ApiBlueprint.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiBlueprint.DataAccess.EntityConfigurations;

public sealed class ProjectConfiguration : EntityConfigurationBase<Project, Guid>
{
    public override void Configure(EntityTypeBuilder<Project> builder)
    {
        base.Configure(builder);

        builder.Property(project => project.Name).IsRequired();
        builder.Property(project => project.Description);
        builder.Property(project => project.ImageUrl);
        builder.Property(project => project.CreatedAtUtc).IsRequired();
        builder.Property(project => project.UpdatedAtUtc).IsRequired();

        builder.HasMany(project => project.ProjectMembers)
            .WithOne(member => member.Project)
            .HasForeignKey(member => member.ProjectId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}