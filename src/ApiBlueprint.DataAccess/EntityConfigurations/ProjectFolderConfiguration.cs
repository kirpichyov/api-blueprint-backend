using System;
using ApiBlueprint.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiBlueprint.DataAccess.EntityConfigurations;

public sealed class ProjectFolderConfiguration : EntityConfigurationBase<ProjectFolder, Guid>
{
    public override void Configure(EntityTypeBuilder<ProjectFolder> builder)
    {
        base.Configure(builder);

        builder.Property(folder => folder.Name).IsRequired();
        builder.Property(folder => folder.CreatedAtUtc).IsRequired();
        
        builder.HasOne(folder => folder.Project)
            .WithMany(project => project.ProjectFolders)
            .HasForeignKey(folder => folder.ProjectId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}