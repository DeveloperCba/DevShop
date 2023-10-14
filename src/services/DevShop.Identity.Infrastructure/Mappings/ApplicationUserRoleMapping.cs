using DevShop.Identity.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevShop.Identity.Infrastructure.Mappings;

public class ApplicationUserRoleMapping : IEntityTypeConfiguration<ApplicationUserRole>
{
    public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
    {
        builder.ToTable("AuthUserRole");
        builder.HasKey(x => new { x.UserId, x.RoleId });
        builder.HasIndex(x => x.UserId, name: "IX_AuthUserRole_UserId");

        builder.Property(x => x.UserId).HasMaxLength(450).IsRequired();
        builder.Property(x => x.RoleId).HasMaxLength(450).IsRequired();
    }
}