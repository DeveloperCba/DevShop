using DevShop.Identity.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevShop.Identity.Infrastructure.Mappings;

public class ApplicationRoleClaimMapping : IEntityTypeConfiguration<ApplicationRoleClaim>
{
    public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
    {
        builder.ToTable("AuthRoleClaims");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.RoleId, name: "IX_AuthRoleClaims_RoleId");

        builder.Property(x => x.RoleId).HasMaxLength(450).IsRequired();
        builder.Property(x => x.ClaimType).HasMaxLength(8000).IsRequired();
        builder.Property(x => x.ClaimValue).HasMaxLength(8000);
    }
}