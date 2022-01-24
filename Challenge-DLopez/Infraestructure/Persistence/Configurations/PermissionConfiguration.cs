using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Persistence.Configurations
{
	public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
	{
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions", "dbo");

            builder.HasKey(t => t.Id)
                .HasName("PK_Permissions");

            builder.Property(t => t.EmployeeForename)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(t => t.EmployeeSurname)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(t => t.PermissionType)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength(true)
                .IsRequired();

            builder.Property(t => t.PermissionDate)
                .HasColumnType("datetime")
                .IsRequired();

            builder.HasOne(d => d.PermissionTypeNavigation)
               .WithMany(p => p.Permissions)
               .HasForeignKey(d => d.PermissionType)
               .HasPrincipalKey(p => p.Id)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_Permissions_PermissionType");
        }
    }
}
