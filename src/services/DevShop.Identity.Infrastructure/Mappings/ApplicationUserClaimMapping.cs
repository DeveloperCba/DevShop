using DevShop.Identity.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevShop.Identity.Infrastructure.Mappings;

public class ApplicationUserClaimMapping : IEntityTypeConfiguration<ApplicationUserClaim>
{
    public void Configure(EntityTypeBuilder<ApplicationUserClaim> builder)
    {
        builder.ToTable("AuthUserClaim");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.UserId, name: "IX_AuthUserClaim_UserId");

        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.UserId).HasMaxLength(450).IsRequired();
        builder.Property(x => x.ClaimType).HasMaxLength(8000);
        builder.Property(x => x.ClaimValue).HasMaxLength(8000);
    }
}