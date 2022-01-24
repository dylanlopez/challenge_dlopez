using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Persistence.Configurations
{
	public class PermissionTypeConfiguration : IEntityTypeConfiguration<PermissionType>
    {
        public void Configure(EntityTypeBuilder<PermissionType> builder)
        {
            builder.ToTable("PermissionType", "dbo");

            builder.HasKey(t => t.Id)
                .HasName("PK_PermissionTypes");

            builder.Property(t => t.Description)
                .HasMaxLength(500)
                .IsUnicode(true)
                .IsRequired();

            builder.HasMany(d => d.Permissions)
                .WithOne(p => p.PermissionTypeNavigation)
                .HasForeignKey(d => d.PermissionType)
                .HasPrincipalKey(p => p.Id);
        }
    }
}
