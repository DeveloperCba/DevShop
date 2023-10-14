using DevShop.Identity.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevShop.Identity.Infrastructure.Mappings;

public class ApplicationRoleMapping : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.ToTable("AuthRole");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.NormalizedName, name: "RoleNameIndex").IsUnique();

        builder.Property(x => x.Id).HasMaxLength(450).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(256);
        builder.Property(x => x.NormalizedName).HasMaxLength(256);
        builder.Property(x => x.ConcurrencyStamp).HasMaxLength(8000);

        // Each Role can have many entries in the UserRole join table
        builder.HasMany(e => e.UserRoles)
            .WithOne(e => e.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();

        // Each Role can have many associated RoleClaims
        builder.HasMany(e => e.RoleClaims)
            .WithOne(e => e.Role)
            .HasForeignKey(rc => rc.RoleId)
            .IsRequired();
    }
}